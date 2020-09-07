
using System;

namespace CaseManagement.CMMN.Domains
{
    public class MilestoneElementInstance : CasePlanElementInstance
    {
        public MilestoneEventStates? State { get; set; }

        protected override void UpdateTransition(CMMNTransitions transition, DateTime updateDateTime)
        {
            State = GetMilestoneOrEventListenerState(State, transition);
        }
    }
}
