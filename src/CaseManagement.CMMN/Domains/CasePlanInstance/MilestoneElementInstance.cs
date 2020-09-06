
namespace CaseManagement.CMMN.Domains
{
    public class MilestoneElementInstance : CasePlanElementInstance
    {
        public MilestoneEventStates? State { get; set; }

        protected override void UpdateTransition(CMMNTransitions transition)
        {
            State = GetMilestoneOrEventListenerState(State, transition);
        }
    }
}
