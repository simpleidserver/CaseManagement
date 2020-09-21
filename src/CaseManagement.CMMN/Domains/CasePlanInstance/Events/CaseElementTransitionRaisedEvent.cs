using CaseManagement.Common.Domains;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains
{
    [DebuggerDisplay("Make transition '{Transition}' to element '{ElementId}'")]
    [Serializable]
    public class CaseElementTransitionRaisedEvent : DomainEvent
    {
        public CaseElementTransitionRaisedEvent(string id, string aggregateId, int version, string elementId, CMMNTransitions transition, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            Transition = transition;
            UpdateDateTime = updateDateTime;
        }

        public string ElementId { get; set; }
        public CMMNTransitions Transition { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
