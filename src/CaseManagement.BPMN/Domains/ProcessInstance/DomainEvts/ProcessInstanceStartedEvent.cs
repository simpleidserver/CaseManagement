using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Start process instance")]
    public class ProcessInstanceStartedEvent : DomainEvent
    {
        public ProcessInstanceStartedEvent(string id, string aggregateId, int version, string nameIdentifier, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            UpdateDateTime = updateDateTime;
            NameIdentifier = nameIdentifier;
        }

        public DateTime UpdateDateTime { get; set; }
        public string NameIdentifier { get; set; }
    }
}
