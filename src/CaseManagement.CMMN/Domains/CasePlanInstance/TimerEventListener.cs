using System;

namespace CaseManagement.CMMN.Domains
{
    public class TimerEventListener : CasePlanElementInstance
    {
        public MilestoneEventStates? State { get; set; }
        public CMMNExpression TimerExpression { get; set; }

        protected override void UpdateTransition(CMMNTransitions transition, DateTime executionDateTime)
        {
            State = GetMilestoneOrEventListenerState(State, transition);
        }

        public object Clone()
        {
            return new TimerEventListener
            {
                Id = Id,
                Name = Name,
                TimerExpression = TimerExpression == null ? null : (CMMNExpression)TimerExpression.Clone()
            };
        }
    }
}
