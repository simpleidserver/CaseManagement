using CaseManagement.Workflow.Infrastructure;
using System;

namespace CaseManagement.Workflow.Domains.Events
{
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
