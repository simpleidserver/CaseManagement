using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.EvtBus;

namespace CaseManagement.Workflow.Domains.Events
{
    public class ProcessFlowInstanceCompletedEvent : DomainEvent
    {
        public ProcessFlowInstanceCompletedEvent(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
