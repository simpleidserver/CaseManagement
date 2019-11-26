using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceFormConfirmedEvent : DomainEvent
    {
        public ProcessFlowInstanceFormConfirmedEvent(string id, string elementId)
        {
            Id = id;
            ElementId = elementId;
        }

        public string Id { get; set; }
        public string ElementId { get; set; }
    }
}
