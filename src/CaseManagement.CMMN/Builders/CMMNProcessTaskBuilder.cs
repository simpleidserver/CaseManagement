using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class CMMNProcessTaskBuilder : CMMNWorkflowElementBuilder
    {
        public CMMNProcessTaskBuilder(CMMNPlanItemDefinition planItem) : base(planItem)
        {
        }

        public CMMNProcessTaskBuilder SetIsBlocking(bool isBlocking)
        {
            var cmmnTask = (WorkflowElementDefinition as CMMNPlanItemDefinition).PlanItemDefinitionProcessTask;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }

        public CMMNProcessTaskBuilder SetProcessRef(string processRef)
        {
            var cmmnTask = (WorkflowElementDefinition as CMMNPlanItemDefinition).PlanItemDefinitionProcessTask;
            cmmnTask.ProcessRef = processRef;
            return this;
        }

        public CMMNProcessTaskBuilder AddMapping(string sourceRef, string targetRef)
        {
            var cmmnTask = (WorkflowElementDefinition as CMMNPlanItemDefinition).PlanItemDefinitionProcessTask;
            cmmnTask.Mappings.Add(new CMMNParameterMapping
            {
                SourceRef = new CMMNParameter { Name = sourceRef },
                TargetRef = new CMMNParameter { Name = targetRef }
            });
            return this;
        }
    }
}
