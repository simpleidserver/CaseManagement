using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceCompletedEvent : DomainEvent
    {
        public ProcessFlowInstanceCompletedEvent(string id, string aggregateId, int version) : base(id, aggregateId, version)
        {
        }
    }
}
