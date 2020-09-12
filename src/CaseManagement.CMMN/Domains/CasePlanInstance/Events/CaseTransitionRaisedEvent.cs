using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    [DebuggerDisplay("Make transition '{Transition}'")]
    public class CaseTransitionRaisedEvent : DomainEvent
    {
        public CaseTransitionRaisedEvent(string id, string aggregateId, int version, CMMNTransitions transition, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            Transition = transition;
            UpdateDateTime = updateDateTime;
        }

        public CMMNTransitions Transition { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
