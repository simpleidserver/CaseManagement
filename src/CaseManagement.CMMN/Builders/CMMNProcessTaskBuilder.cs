using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNProcessTaskBuilder : CMMNTaskBuilder
    {
        public CMMNProcessTaskBuilder(CMMNPlanItem planItem) : base(planItem)
        {
        }

        public CMMNProcessTaskBuilder SetProcessRef(string processRef)
        {
            var cmmnTask = (CMMNProcessTask)PlanItem.PlanItemDefinition;
            cmmnTask.ProcessRef = processRef;
            return this;
        }

        public CMMNProcessTaskBuilder AddMapping(string sourceRef, string targetRef)
        {
            var cmmnTask = (CMMNProcessTask)PlanItem.PlanItemDefinition;
            cmmnTask.Mappings.Add(new CMMNParameterMapping
            {
                SourceRef = new CMMNParameter { Name = sourceRef },
                TargetRef = new CMMNParameter { Name = targetRef }
            });
            return this;
        }
    }
}
