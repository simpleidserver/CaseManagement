using CaseManagement.Workflow.Infrastructure;

namespace CaseManagement.CMMN.Domains.Events
{
    public class CMMNPlanItemTransitionEvent : DomainEvent
    {
        public CMMNPlanItemTransitionEvent(CMMNPlanItemTransitions transition)
        {
            Transition = transition;
        }

        public CMMNPlanItemTransitions Transition { get; set; }
    }
}
