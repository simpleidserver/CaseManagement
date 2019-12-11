using CaseManagement.Workflow.Infrastructure;
using System.Diagnostics;

namespace CaseManagement.Workflow.Domains.Events
{
    [DebuggerDisplay("Cancel process flow {AggregateId}")]
    public class ProcessFlowInstanceCanceledEvent : DomainEvent
    {
        public ProcessFlowInstanceCanceledEvent(string id, string aggregateId, int version) : base(id, aggregateId, version)
        {
        }
    }
}
