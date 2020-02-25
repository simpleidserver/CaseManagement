using CaseManagement.CMMN.Infrastructures;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Form '{AggregateId}' is published")]
    public class FormPublishedEvent : DomainEvent
    {
        public FormPublishedEvent(string id, string aggregateId, int version, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            UpdateDateTime = updateDateTime;
        }

        public DateTime UpdateDateTime { get; set; }
    }
}
