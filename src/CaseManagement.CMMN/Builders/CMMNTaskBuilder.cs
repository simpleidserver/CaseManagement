using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNTaskBuilder : CMMNPlanItemBuilder
    {
        public CMMNTaskBuilder(CMMNPlanItemDefinition planItem) : base(planItem)
        {
        }

        public CMMNTaskBuilder SetIsBlocking(bool isBlocking)
        {
            var cmmnTask = PlanItem.PlanItemDefinitionTask;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }
    }
}
