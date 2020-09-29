using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.HumanTask.Domains
{
    [DebuggerDisplay("Activate")]
    public class ActivatedEvent : DomainEvent
    {
        public ActivatedEvent(string id, string aggregateId, int version, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            UpdateDateTime = updateDateTime;
        }

        public DateTime UpdateDateTime { get; set; }
    }
}
