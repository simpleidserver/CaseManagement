using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public abstract class BaseMilestoneOrTimerElementInstance : BaseCasePlanItemInstance
    {
        #region Properties

        public MilestoneEventStates? State { get; set; }

        #endregion

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