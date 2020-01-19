using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Builders
{
    public class ProcessTaskBuilder : WorkflowElementBuilder
    {
        public ProcessTaskBuilder(PlanItemDefinition planItem) : base(planItem)
        {
        }

        public ProcessTaskBuilder SetIsBlocking(bool isBlocking)
        {
            var cmmnTask = (WorkflowElementDefinition as PlanItemDefinition).PlanItemDefinitionProcessTask;
            cmmnTask.IsBlocking = isBlocking;
            return this;
        }

        public ProcessTaskBuilder SetProcessRef(string processRef)
        {
            var cmmnTask = (WorkflowElementDefinition as PlanItemDefinition).PlanItemDefinitionProcessTask;
            cmmnTask.ProcessRef = processRef;
            return this;
        }

        public ProcessTaskBuilder AddMapping(string sourceRef, string targetRef)
        {
            var cmmnTask = (WorkflowElementDefinition as PlanItemDefinition).PlanItemDefinitionProcessTask;
            cmmnTask.Mappings.Add(new ParameterMapping
            {
                SourceRef = new CMMNParameter { Name = sourceRef },
                TargetRef = new CMMNParameter { Name = targetRef }
            });
            return this;
        }

        public ProcessTaskBuilder AddCaseInstanceIdInputMapping()
        {
            var cmmnTask = (WorkflowElementDefinition as PlanItemDefinition).PlanItemDefinitionProcessTask;
            cmmnTask.Mappings.Add(new ParameterMapping
            {
                SourceRef = new CMMNParameter { Name = CMMNConstants.StandardProcessMappingVariables.CaseInstanceId },
                TargetRef = new CMMNParameter { Name = CMMNConstants.StandardProcessMappingVariables.CaseInstanceId }
            });
            return this;
        }
    }
}
