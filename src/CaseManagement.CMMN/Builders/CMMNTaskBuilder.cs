using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNTaskBuilder : CMMNWorkflowElementBuilder
    {
        public CMMNTaskBuilder(CMMNPlanItemDefinition planItem) : base(planItem)
        {
        }

        public CMMNTaskBuilder SetIsBlocking(bool isBlocking)
        {
            var cmmnTask = (WorkflowElementDefinition as CMMNPlanItemDefinition).PlanItemDefinitionTask;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }
    }
}
