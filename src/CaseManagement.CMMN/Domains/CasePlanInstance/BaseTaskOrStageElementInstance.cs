using System;

namespace CaseManagement.CMMN.Domains
{
    public class BaseTaskOrStageElementInstance : CasePlanElementInstance
    {
        public ManualActivationRule ManualActivationRule { get; set; }
        public TaskStageStates? State { get; set; }
        public bool IsBlocking { get; set; }

        public bool IsManualActivationRuleSatisfied()
        {
            return ManualActivationRule.IsSatisfied();
        }

        protected override void UpdateTransition(CMMNTransitions transition, DateTime executionDateTime)
        {
            if (transition == CMMNTransitions.ParentTerminate)
            {
                if (State != TaskStageStates.Completed && State != TaskStageStates.Terminated)
                {
                    State = TaskStageStates.Terminated;
                }
            }
            else
            {
                State = GetTaskStageState(State, transition);
            }

            Process(transition, executionDateTime);
        }

        protected virtual void Process(CMMNTransitions transition, DateTime executionDateTime) { }
    }
}
