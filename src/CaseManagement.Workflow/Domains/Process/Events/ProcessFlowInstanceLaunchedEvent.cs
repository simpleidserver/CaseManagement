using CaseManagement.Workflow.Infrastructure;
using System.Diagnostics;

namespace CaseManagement.Workflow.Domains.Events
{
    [DebuggerDisplay("Launch {AggregateId} process")]
    public class ProcessFlowInstanceLaunchedEvent : DomainEvent
    {
        public ProcessFlowInstanceLaunchedEvent(string id, string aggregateId, int version) : base(id, aggregateId, version)
        {
        }
    }
}
