using CaseManagement.CMMN.Domains.Events;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Parser;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.CMMN.Domains
{
    public class CaseFileAggregate : BaseAggregate
    {
        public string FileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string Payload { get; set; }
        public string Owner { get; set; }
        public CaseFileStatus Status { get; set; }

        public void Update(string name, string description, string payload)
        {
            var evt = new CaseFileUpdatedEvent(Guid.NewGuid().ToString(), Id, Version, DateTime.UtcNow, name, description, payload);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public CaseFileAggregate Publish()
        {
            var evt = new CaseFilePublishedEvent(Guid.NewGuid().ToString(), Id, Version);
            Handle(evt);
            DomainEvents.Add(evt);
            var next = New(Name, Description, Version + 1, Owner, Payload, FileId);
            return next;
        }

        public static CaseFileAggregate New(List<DomainEvent> evts)
        {
            var result = new CaseFileAggregate();
            foreach(var evt in evts)
            {
                result.Handle(evt);
            }

            return result;
        }

        public static CaseFileAggregate New(string name, string description, int version, string owner, string payload = null, string fileId = null)
        {
            var result = new CaseFileAggregate();
            var evt = new CaseFileAddedEvent(Guid.NewGuid().ToString(), BuildCaseFileIdentifier(fileId, version), version, fileId, name, description, DateTime.UtcNow, owner, payload);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public override void Handle(dynamic obj)
        {
            Handle(obj);
        }

        private void Handle(CaseFileAddedEvent caseFileAddedEvent)
        {
            if (string.IsNullOrWhiteSpace(caseFileAddedEvent.Name))
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "name must be specified" }
                });
            }

            if (string.IsNullOrWhiteSpace(caseFileAddedEvent.Description))
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "description must be specified" }
                });
            }

            Id = caseFileAddedEvent.AggregateId;
            FileId = caseFileAddedEvent.FileId;
            Name = caseFileAddedEvent.Name;
            Description = caseFileAddedEvent.Description;
            CreateDateTime = caseFileAddedEvent.CreateDateTime;
            Owner = caseFileAddedEvent.Owner;
            Payload = caseFileAddedEvent.Payload;
            Version = caseFileAddedEvent.Version;
            Status = CaseFileStatus.Edited;
        }

        private void Handle(CaseFileUpdatedEvent caseFileUpdatedEvent)
        {
            if (string.IsNullOrWhiteSpace(caseFileUpdatedEvent.Name))
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "name must be specified" }
                });
            }

            if (string.IsNullOrWhiteSpace(caseFileUpdatedEvent.Description))
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "description must be specified" }
                });
            }

            if (string.IsNullOrWhiteSpace(caseFileUpdatedEvent.Payload))
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "payload must be specified" }
                });
            }

            try
            {
                CMMNParser.ParseWSDL(caseFileUpdatedEvent.Payload);
            }
            catch
            {
                throw new AggregateValidationException(new Dictionary<string, string>
                {
                    { "validation", "xml file is not valid" }
                });
            }

            UpdateDateTime = caseFileUpdatedEvent.UpdateDatetime;
            Name = caseFileUpdatedEvent.Name;
            Description = caseFileUpdatedEvent.Description;
            Payload = caseFileUpdatedEvent.Payload;
        }

        private void Handle(CaseFilePublishedEvent caseFilePublishedEvent)
        {
            Status = CaseFileStatus.Published;
        }

        public override object Clone()
        {
            return new CaseFileAggregate
            {
                Id = Id,
                FileId = FileId,
                Name = Name,
                Description = Description,
                CreateDateTime = CreateDateTime,
                Payload = Payload,
                Owner = Owner,
                UpdateDateTime = UpdateDateTime,
                Status = Status,
                Version = Version
            };
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        
        public static string GetStreamName(string id)
        {
            return $"case-file-{id}";
        }

        public static string BuildCaseFileIdentifier(string fileId, int version)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{fileId}{version}"));
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