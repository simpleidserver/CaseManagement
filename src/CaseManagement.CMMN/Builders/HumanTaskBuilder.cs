using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class HumanTaskBuilder : WorkflowElementBuilder
    {
        public HumanTaskBuilder(PlanItemDefinition planItem) : base(planItem)
        {
        }

        public HumanTaskBuilder SetIsBlocking(bool isBlocking)
        {
            var cmmnTask = (WorkflowElementDefinition as PlanItemDefinition).PlanItemDefinitionHumanTask;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }

        public HumanTaskBuilder SetFormId(string formId)
        {
            var cmmnTask = (WorkflowElementDefinition as PlanItemDefinition).PlanItemDefinitionHumanTask;
            cmmnTask.FormId = formId;
            return this;
        }

        public HumanTaskBuilder SetPerformerRef(string performerRef)
        {
            var cmmnTask = (WorkflowElementDefinition as PlanItemDefinition).PlanItemDefinitionHumanTask;
            cmmnTask.PerformerRef = performerRef;
            return this;
        }
    }
}
