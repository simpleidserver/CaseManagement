using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNHumanTaskBuilder : CMMNWorkflowElementBuilder
    {
        public CMMNHumanTaskBuilder(CMMNPlanItemDefinition planItem) : base(planItem)
        {
        }

        public CMMNHumanTaskBuilder SetIsBlocking(bool isBlocking)
        {
            var cmmnTask = (WorkflowElementDefinition as CMMNPlanItemDefinition).PlanItemDefinitionHumanTask;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }

        public CMMNHumanTaskBuilder SetFormId(string formId)
        {
            var cmmnTask = (WorkflowElementDefinition as CMMNPlanItemDefinition).PlanItemDefinitionHumanTask;
            cmmnTask.FormId = formId;
            return this;
        }
    }
}
