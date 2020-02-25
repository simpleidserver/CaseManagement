using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.CMMN.Domains
{
    public class FormAggregate : BaseAggregate
    {
        public FormAggregate()
        {
            Titles = new List<Translation>();
            Elements = new List<FormElement>();
            Status = FormStatus.Edited;
        }

        public string FormId { get; set; }
        public ICollection<Translation> Titles { get; set; }
        public ICollection<FormElement> Elements { get; set; }
        public FormStatus Status { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public static FormAggregate New(ICollection<Translation> titles, ICollection<FormElement> elements)
        {
            var result = new FormAggregate();
            lock(result.DomainEvents)
            {
                var formId = Guid.NewGuid().ToString();
                var aggregateId = BuildIdentifier(formId, 0);
                var evt = new FormAddedEvent(Guid.NewGuid().ToString(), aggregateId, 0, DateTime.UtcNow, formId, titles, elements);
                result.Handle(evt);
                result.DomainEvents.Add(evt);
            }

            return result;
        }

        public void Update(ICollection<Translation> titles, ICollection<FormElement> elements)
        {
            lock(DomainEvents)
            {
                var evt = new FormUpdatedEvent(Guid.NewGuid().ToString(), Id, Version, DateTime.UtcNow, titles, elements);
                Handle(evt);
                DomainEvents.Add(evt);
            }
        }

        public FormAggregate Publish()
        {
            lock(DomainEvents)
            {
                var evt = new FormPublishedEvent(Guid.NewGuid().ToString(), Id, Version, DateTime.UtcNow);
                Handle(evt);
                DomainEvents.Add(evt);
            }

            var result = new FormAggregate();
            int nextVersion = Version + 1;
            var aggregateId = BuildIdentifier(FormId, nextVersion);
            var addEvt = new FormAddedEvent(Guid.NewGuid().ToString(), aggregateId, nextVersion, DateTime.UtcNow, FormId, Titles, Elements);
            result.Handle(addEvt);
            result.DomainEvents.Add(addEvt);
            return result;
        }

        public override object Clone()
        {
            return new FormAggregate
            {
                Id =  Id,
                FormId = FormId,
                Version = Version,
                Elements = Elements.Select(e => e.Clone() as FormElement).ToList(),
                Titles = Titles.Select(e => e.Clone() as Translation).ToList(),
                CreateDateTime = CreateDateTime,
                UpdateDateTime = UpdateDateTime,
                Status = Status
            };
        }

        public override void Handle(object obj)
        {
            if (obj is FormAddedEvent)
            {
                Handle(obj as FormAddedEvent);
            }

            if (obj is FormUpdatedEvent)
            {
                Handle(obj as FormUpdatedEvent);
            }

            if (obj is FormPublishedEvent)
            {
                Handle(obj as FormPublishedEvent);
            }
        }

        private void Handle(FormAddedEvent formAddedEvent)
        {
            Id = formAddedEvent.AggregateId;
            Version = formAddedEvent.Version;
            CreateDateTime = formAddedEvent.CreateDateTime;
            FormId = formAddedEvent.FormId;
            Titles = formAddedEvent.Titles;
            Elements = formAddedEvent.Elements;
            Status = FormStatus.Edited;
        }

        private void Handle(FormUpdatedEvent formUpdatedEvent)
        {
            UpdateDateTime = formUpdatedEvent.UpdateDateTime;
            Titles = formUpdatedEvent.Titles;
            Elements = formUpdatedEvent.Elements;
            Status = FormStatus.Edited;
        }

        private void Handle(FormPublishedEvent formPublishedEvent)
        {
            UpdateDateTime = formPublishedEvent.UpdateDateTime;
            Version = formPublishedEvent.Version;
            Status = FormStatus.Published;
        }

        public static string BuildIdentifier(string id, int version)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{id}{version}"));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
