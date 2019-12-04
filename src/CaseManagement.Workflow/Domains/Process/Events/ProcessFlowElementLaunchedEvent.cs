using CaseManagement.Workflow.Infrastructure;
using System;

namespace CaseManagement.Workflow.Domains.Events
{
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
