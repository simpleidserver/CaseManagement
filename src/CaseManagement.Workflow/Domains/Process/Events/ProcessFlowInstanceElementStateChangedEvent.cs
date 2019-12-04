using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceElementStateChangedEvent : DomainEvent
    {
        public ProcessFlowInstanceElementStateChangedEvent(string id, string aggregateId, int version, string elementId, string state) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            State = state;
        }

        public string ElementId { get; set; }
        public string State { get; set; }
    }
}
