using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Update info")]
    public class HumanTaskInfoUpdatedEvent : DomainEvent
    {
        public HumanTaskInfoUpdatedEvent(string id, string aggregateId, int version, string name, int priority, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            Name = name;
            Priority = priority;
            UpdateDateTime = updateDateTime;
        }

        public string Name { get; set; }
        public int Priority { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
