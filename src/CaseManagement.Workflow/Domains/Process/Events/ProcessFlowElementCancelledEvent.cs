using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.Workflow.Domains.Events
{
    [DebuggerDisplay("Element {ElementId} is cancelled")]
    public class ProcessFlowElementCancelledEvent : DomainEvent
    {
        public ProcessFlowElementCancelledEvent(string id, string aggregateId, int version, string elementId, DateTime cancellationDateTime) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            CancellationDateTime = cancellationDateTime;
        }

        public string ElementId { get; set; }
        public DateTime CancellationDateTime { get; set; }
    }
}