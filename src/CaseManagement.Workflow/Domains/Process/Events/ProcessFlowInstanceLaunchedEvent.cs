using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceLaunchedEvent : DomainEvent
    {
        public ProcessFlowInstanceLaunchedEvent(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
