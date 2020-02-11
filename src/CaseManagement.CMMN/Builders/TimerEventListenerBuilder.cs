using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class TimerEventListenerBuilder : WorkflowElementBuilder
    {
        public TimerEventListenerBuilder(CasePlanElement workflowElementDef) : base(workflowElementDef)
        {
        }

        public TimerEventListenerBuilder SetTimer(CMMNExpression expression)
        {
            var timer = (WorkflowElementDefinition as PlanItemDefinition).PlanItemDefinitionTimerEventListener;
            timer.TimerExpression = expression;
            return this;
        }
    }
}
