using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Update process definition")]
    [Serializable]
    public class ProcessFileUpdatedEvent : DomainEvent
    {
        public ProcessFileUpdatedEvent(string id, string aggregateId, int version, string name, string description, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            Name = name;
            Description = description;
            UpdateDateTime = updateDateTime;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
