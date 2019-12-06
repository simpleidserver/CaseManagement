using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.Workflow.Domains.Events
{
    [DebuggerDisplay("Start element {ElementId}")]
    public class ProcessFlowElementStartedEvent : DomainEvent
    {
        public ProcessFlowElementStartedEvent(string id, string aggregateId, int version, string elementId, DateTime startDateTime) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            StartDateTime = startDateTime;
        }
        
        public string ElementId { get; set; }
        public DateTime StartDateTime { get; set; }
    }
}
