
namespace CaseManagement.CMMN.Domains
{
    public class BaseTaskOrStageElementInstance : CasePlanElementInstance
    {
        public ManualActivationRule ManualActivationRule { get; set; }
        public TaskStageStates? State { get; set; }
        public bool IsBlocking { get; set; }

        protected override void UpdateTransition(CMMNTransitions transition)
        {
            State = GetTaskStageState(State, transition);
        }
    }
}
