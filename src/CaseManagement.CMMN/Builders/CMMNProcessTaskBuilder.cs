using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNProcessTaskBuilder : CMMNPlanItemBuilder
    {
        public CMMNProcessTaskBuilder(CMMNPlanItem planItem) : base(planItem)
        {
        }

        public CMMNProcessTaskBuilder SetIsBlocking(bool isBlocking)
        {
            var cmmnTask = PlanItem.PlanItemDefinitionProcessTask;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }

        public CMMNProcessTaskBuilder SetState(CMMNTaskStates state)
        {
            var cmmnTask = PlanItem.PlanItemDefinitionProcessTask;
            cmmnTask.State = state;
            return this;
        }

        public CMMNProcessTaskBuilder SetProcessRef(string processRef)
        {
            var cmmnTask = PlanItem.PlanItemDefinitionProcessTask;
            cmmnTask.ProcessRef = processRef;
            return this;
        }

        public CMMNProcessTaskBuilder AddMapping(string sourceRef, string targetRef)
        {
            var cmmnTask = PlanItem.PlanItemDefinitionProcessTask;
            cmmnTask.Mappings.Add(new CMMNParameterMapping
            {
                SourceRef = new CMMNParameter { Name = sourceRef },
                TargetRef = new CMMNParameter { Name = targetRef }
            });
            return this;
        }
    }
}
