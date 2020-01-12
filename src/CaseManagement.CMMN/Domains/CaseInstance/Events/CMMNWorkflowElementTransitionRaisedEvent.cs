using CaseManagement.Workflow.Infrastructure;
using System;
using System.Diagnostics;

namespace CaseManagement.CMMN.Domains.Events
{
    [DebuggerDisplay("Make transition {Transition} to element {ElementId}")]
    public class CMMNWorkflowElementTransitionRaisedEvent : DomainEvent
    {
        public CMMNWorkflowElementTransitionRaisedEvent(string id, string aggregateId, int version, string elementId, string elementDefinitionId, CMMNTransitions transition, DateTime updateDateTime) : base(id, aggregateId, version)
        {
            ElementId = elementId;
            ElementDefinitionId = elementDefinitionId;
            Transition = transition;
            UpdateDateTime = updateDateTime;
            Version = version;
        }

        public string ElementId { get; set; }
        public string ElementDefinitionId { get; set; }
        public CMMNTransitions Transition { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
