using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Make transition {Transition} to element {CaseElementId}")]
    public class CaseElementTransitionRaisedEvent : DomainEvent
    {
        public CaseElementTransitionRaisedEvent(string id, string aggregateId, int version, string elementId, string elementDefinitionId, CMMNTransitions transition, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            CaseElementId = elementId;
            CaseElementDefinitionId = elementDefinitionId;
            Transition = transition;
            UpdateDateTime = updateDateTime;
            Version = version;
        }

        public string CaseElementId { get; set; }
        public string CaseElementDefinitionId { get; set; }
        public CMMNTransitions Transition { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
