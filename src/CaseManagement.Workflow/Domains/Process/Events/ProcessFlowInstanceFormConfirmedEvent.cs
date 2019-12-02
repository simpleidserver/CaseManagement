using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceFormConfirmedEvent : DomainEvent
    {
        public ProcessFlowInstanceFormConfirmedEvent(string id, string elementId, ProcessFlowInstanceElementForm formInstance)
        {
            Id = id;
            ElementId = elementId;
            FormInstance = formInstance;
        }

        public string Id { get; set; }
        public string ElementId { get; set; }
        public ProcessFlowInstanceElementForm FormInstance { get; set; }
    }
}
