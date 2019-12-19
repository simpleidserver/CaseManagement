using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNHumanTaskBuilder : CMMNPlanItemBuilder
    {
        public CMMNHumanTaskBuilder(CMMNPlanItemDefinition planItem) : base(planItem)
        {
        }

        public CMMNHumanTaskBuilder SetIsBlocking(bool isBlocking)
        {
            var cmmnTask = PlanItem.PlanItemDefinitionHumanTask;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }

        public CMMNHumanTaskBuilder SetFormId(string formId)
        {
            var cmmnTask = PlanItem.PlanItemDefinitionHumanTask;
            cmmnTask.FormId = formId;
            return this;
        }
    }
}
