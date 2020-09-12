using System;

namespace CaseManagement.CMMN.Domains
{
    public abstract class BaseMilestoneOrTimerElementInstance : CasePlanElementInstance
    {
        public MilestoneEventStates? State { get; set; }

        protected override void UpdateTransition(CMMNTransitions transition, DateTime executionDateTime)
        {
            var newState = GetMilestoneOrEventListenerState(State, transition);
            if (newState != null)
            {
                State = newState;
            }
        }

        protected void FeedMilestoneOrTimer(BaseMilestoneOrTimerElementInstance elt)
        {
            elt.State = State;
        }
    }
}