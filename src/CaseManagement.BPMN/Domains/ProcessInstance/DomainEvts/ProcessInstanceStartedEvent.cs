using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Start process instance")]
    public class ProcessInstanceStartedEvent : DomainEvent
    {
        public ProcessInstanceStartedEvent(string id, string aggregateId, int version, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            UpdateDateTime = updateDateTime;
        }

        public DateTime UpdateDateTime { get; set; }
    }
}
