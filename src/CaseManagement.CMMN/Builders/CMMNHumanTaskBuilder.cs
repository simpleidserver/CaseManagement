using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNHumanTaskBuilder : CMMNPlanItemBuilder
    {
        public CMMNHumanTaskBuilder(CMMNPlanItem planItem) : base(planItem)
        {
        }

        public CMMNHumanTaskBuilder SetIsBlocking(bool isBlocking)
        {
            var cmmnTask = PlanItem.PlanItemDefinitionHumanTask;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }

        public CMMNHumanTaskBuilder SetState(CMMNTaskStates state)
        {
            var cmmnTask = PlanItem.PlanItemDefinitionHumanTask;
            cmmnTask.State = state;
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
