using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNTimerEventListenerBuilder : CMMNWorkflowElementBuilder
    {
        public CMMNTimerEventListenerBuilder(CMMNWorkflowElementDefinition workflowElementDef) : base(workflowElementDef)
        {
        }

        public CMMNTimerEventListenerBuilder SetTimer(CMMNExpression expression)
        {
            var timer = (WorkflowElementDefinition as CMMNPlanItemDefinition).PlanItemDefinitionTimerEventListener;
            timer.TimerExpression = expression;
            return this;
        }
    }
}
