using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public abstract class BaseTaskOrStageElementInstance : BaseCasePlanItemInstance
    {
        #region Properties

        public ManualActivationRule ManualActivationRule { get; set; }
        public TaskStageStates? State { get; set; }
        public bool IsBlocking { get; set; }

        #endregion

        public bool IsManualActivationRuleSatisfied(CasePlanInstanceExecutionContext executionContext)
        {
            return ManualActivationRule.IsSatisfied(executionContext);
        }

        protected void FeedTaskOrStage(BaseTaskOrStageElementInstance elt)
        {
            elt.ManualActivationRule = (ManualActivationRule)ManualActivationRule?.Clone();
            elt.State = State;
            elt.IsBlocking = IsBlocking;
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
        }
    }
}
