using CaseManagement.BPMN.Domains.ProcessDefinition.Events;
using CaseManagement.BPMN.Parser;
using CaseManagement.BPMN.Resources;
using CaseManagement.Common.Domains;
using CaseManagement.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.BPMN.Domains
{
    public class ProcessFileAggregate : BaseAggregate
    {
        public string FileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string Payload { get; set; }
        public ProcessFileStatus Status { get; set; }

        public static ProcessFileAggregate New(string fileId, string name, string description, int version, string payload)
        {
            var result = new ProcessFileAggregate();
            var evt = new ProcessFileAddedEvent(Guid.NewGuid().ToString(), BuildProcessDefinitionIdentifier(fileId, version), version, fileId, name, description, DateTime.UtcNow, payload);
            result.Handle(evt);
            result.DomainEvents.Add(evt);
            return result;
        }

        public static ProcessFileAggregate New(IEnumerable<DomainEvent> domainEvts)
        {
            var result = new ProcessFileAggregate();
            foreach (var domainEvt in domainEvts)
            {
                result.Handle(domainEvt);
            }

            return result;
        }

        public void Update(string name, string description)
        {
            var evt = new ProcessFileUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, name, description, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public void UpdatePayload(string payload)
        {
            var evt = new ProcessFilePayloadUpdatedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, payload, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
        }

        public ProcessFileAggregate Publish()
        {
            var evt = new ProcessFilePublishedEvent(Guid.NewGuid().ToString(), AggregateId, Version + 1, DateTime.UtcNow);
            Handle(evt);
            DomainEvents.Add(evt);
            var next = New(FileId, Name, Description, Version + 1, Payload);
            return next;
        }

        public override object Clone()
        {
            return new ProcessFileAggregate
            {
                FileId = FileId,
                AggregateId = AggregateId,
                CreateDateTime = CreateDateTime,
                Description = Description,
                Name = Name,
                Payload = Payload,
                Status = Status,
                UpdateDateTime = UpdateDateTime,
                Version = Version
            };
        }

        public override void Handle(dynamic evt)
        {
            Handle(evt);
        }

        private void Handle(ProcessFilePublishedEvent evt)
        {
            Status = ProcessFileStatus.Published;
            Version = evt.Version;
            UpdateDateTime = evt.PublishedDateTime;
        }

        public string GetStreamName()
        {
            return GetStreamName(AggregateId);
        }

        public static string GetStreamName(string id)
        {
            return $"file{id}";
        }

        private void Handle(ProcessFileAddedEvent evt)
        {
            AggregateId = evt.AggregateId;
            FileId = evt.FileId;
            Name = evt.Name;
            Description = evt.Description;
            CreateDateTime = evt.CreateDateTime;
            Payload = evt.Payload;
            Version = evt.Version;
            Status = ProcessFileStatus.Edited;
        }

        private void Handle(ProcessFileUpdatedEvent evt)
        {

            Name = evt.Name;
            Description = evt.Description;
            UpdateDateTime = evt.UpdateDateTime;
            Version = evt.Version;
        }

        private void Handle(ProcessFilePayloadUpdatedEvent evt)
        {
            try
            {
                BPMNParser.Parse(evt.Payload);
            }
            catch
            {
                throw new AggregateValidationException(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("validation", Global.BPMNFileNotValid)
                });
            }

            Payload = evt.Payload;
            UpdateDateTime = evt.UpdateDateTime;
        }

        public static string BuildProcessDefinitionIdentifier(string fileId, int version)
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
