using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceFormConfirmedEvent : DomainEvent
    {
        public ProcessFlowInstanceFormConfirmedEvent(string id, string elementId, ProcessFlowInstanceElementForm formInstance, string scopeId = null)
        {
            Id = id;
            ElementId = elementId;
            FormInstance = formInstance;
            ScopeId = scopeId;
        }

        public string Id { get; set; }
        public string ElementId { get; set; }
        public ProcessFlowInstanceElementForm FormInstance { get; set; }
        public string ScopeId { get; set; }
    }
}
