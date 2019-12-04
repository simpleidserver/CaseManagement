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
            var cmmnTask = (CMMNTask)PlanItem.PlanItemDefinition;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }

        public CMMNTaskBuilder SetState(CMMNTaskStates state)
        {
            var cmmnTask = (CMMNTask)PlanItem.PlanItemDefinition;
            cmmnTask.State = state;
            return this;
        }
    }
}
