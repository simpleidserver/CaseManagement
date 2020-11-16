using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Update process definition")]
    public class ProcessFileUpdatedEvent : DomainEvent
    {
        public ProcessFileUpdatedEvent(string id, string aggregateId, int version, string name, string description, string payload, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            Name = name;
            Description = description;
            Payload = payload;
            UpdateDateTime = updateDateTime;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Payload { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
