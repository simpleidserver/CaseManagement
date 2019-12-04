using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceLaunchedEvent : DomainEvent
    {
        public ProcessFlowInstanceLaunchedEvent(string id, string aggregateId, int version) : base(id, aggregateId, version)
        {
        }
    }
}
