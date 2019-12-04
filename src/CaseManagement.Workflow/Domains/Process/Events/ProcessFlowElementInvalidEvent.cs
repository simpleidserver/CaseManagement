using CaseManagement.Workflow.Infrastructure;
using System;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowElementInvalidEvent : DomainEvent
    {
        public ProcessFlowElementInvalidEvent(string id, string aggregateId, int version, string elementId, string errorMessage, DateTime invalidDateTime) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            ErrorMessage = errorMessage;
            InvalidDateTime = invalidDateTime;
        }

        public string ElementId { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime InvalidDateTime { get; set; }
    }
}
