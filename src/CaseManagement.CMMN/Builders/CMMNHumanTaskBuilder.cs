using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNHumanTaskBuilder : CMMNTaskBuilder
    {
        public CMMNHumanTaskBuilder(CMMNPlanItem planItem) : base(planItem)
        {
        }

        public CMMNHumanTaskBuilder SetFormId(string formId)
        {
            var cmmnTask = (CMMNHumanTask)PlanItem.PlanItemDefinition;
            cmmnTask.FormId = formId;
            return this;
        }
    }
}
