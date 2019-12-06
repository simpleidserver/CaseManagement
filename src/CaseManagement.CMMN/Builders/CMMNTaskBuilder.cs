using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNTaskBuilder : CMMNPlanItemBuilder
    {
        public CMMNTaskBuilder(CMMNPlanItem planItem) : base(planItem)
        {
        }

        public CMMNTaskBuilder SetIsBlocking(bool isBlocking)
        {
            var cmmnTask = PlanItem.PlanItemDefinitionTask;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }

        public CMMNTaskBuilder SetState(CMMNTaskStates state)
        {
            var cmmnTask = PlanItem.PlanItemDefinitionTask;
            cmmnTask.State = state;
            return this;
        }
    }
}
