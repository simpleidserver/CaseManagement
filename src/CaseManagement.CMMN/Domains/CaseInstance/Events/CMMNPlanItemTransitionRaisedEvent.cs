using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Make transition {Transition} to CMMNPlanItem {ElementId}")]
    public class CMMNPlanItemTransitionRaisedEvent : DomainEvent
    {
        public CMMNPlanItemTransitionRaisedEvent(string id, string aggregateId, int version, string elementId, CMMNPlanItemTransitions transition, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            Transition = transition;
            UpdateDateTime = updateDateTime;
        }

        public string ElementId { get; set; }
        public CMMNPlanItemTransitions Transition { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
