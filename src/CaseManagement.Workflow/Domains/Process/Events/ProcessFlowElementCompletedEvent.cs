using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.Workflow.Domains.Events
{
    [DebuggerDisplay("Element {ElementId} is complete")]
    public class ProcessFlowElementCompletedEvent : DomainEvent
    {
        public ProcessFlowElementCompletedEvent(string id, string aggregateId, int version, string elementId, DateTime completedDateTime) : base(id, aggregateId, version )
        {
            ElementId = elementId;
            CompletedDateTime = completedDateTime;
        }

        public string ElementId { get; set; }
        public DateTime CompletedDateTime { get; set; }
    }
}
