using CaseManagement.Workflow.Infrastructure;
using System.Diagnostics;

namespace CaseManagement.Workflow.Domains.Events
{
    [DebuggerDisplay("Change element state {ElementId} => {State}")]
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
