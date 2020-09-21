using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.BPMN.Domains
{
    [DebuggerDisplay("Make transition '{Transition}' to element '{TechnicalId}'")]
    public class FlowNodeTransitionRaisedEvent : DomainEvent
    {
        public FlowNodeTransitionRaisedEvent(string id, string aggregateId, int version, string technicalId, BPMNTransitions transition, DateTime executionDateTime) : base(id, aggregateId, version)
        {
            TechnicalId = technicalId;
            Transition = transition;
            ExecutionDateTime = executionDateTime;
        }

        public string TechnicalId { get; set; }
        public BPMNTransitions Transition { get; set; }
        public DateTime ExecutionDateTime { get; set; }
    }
}
