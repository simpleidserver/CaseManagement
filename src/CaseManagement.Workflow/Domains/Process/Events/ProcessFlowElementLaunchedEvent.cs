using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.Workflow.Domains.Events
{
    [DebuggerDisplay("Launch element {ElementId}")]
    public class ProcessFlowElementLaunchedEvent : DomainEvent
    {
        public ProcessFlowElementLaunchedEvent(string id, string aggregateId, int version, string elementId, DateTime startDateTime) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            StartDateTime = startDateTime;
        }

        public string ProcessFlowInstanceId { get; set; }
        public string ElementId { get; set; }
        public DateTime StartDateTime { get; set; }
    }
}
