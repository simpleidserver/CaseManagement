using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class PlanItemProcessorParameter
    {
        public PlanItemProcessorParameter(CMMNWorkflowDefinition workflowDefinition, CMMNWorkflowInstance workflowInstance, CMMNPlanItemInstance planItemInstance)
        {
            WorkflowDefinition = workflowDefinition;
            WorkflowInstance = workflowInstance;
            PlanItemInstance = planItemInstance;
        }

        public CMMNWorkflowDefinition WorkflowDefinition { get; set; }
        public CMMNWorkflowInstance WorkflowInstance { get; set; }
        public CMMNPlanItemInstance PlanItemInstance { get; set; }
    }
}
