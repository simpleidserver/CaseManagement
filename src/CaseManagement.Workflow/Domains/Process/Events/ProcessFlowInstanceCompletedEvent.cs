using CaseManagement.Workflow.Infrastructure;
using System.Diagnostics;

namespace CaseManagement.Workflow.Domains.Events
{
    [DebuggerDisplay("Process {AggregateId} is complete")]
    public class ProcessFlowInstanceCompletedEvent : DomainEvent
    {
        public ProcessFlowInstanceCompletedEvent(string id, string aggregateId, int version) : base(id, aggregateId, version)
        {
        }
    }
}
