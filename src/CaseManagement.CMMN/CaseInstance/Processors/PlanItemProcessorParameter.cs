using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class PlanItemProcessorParameter
    {
        public PlanItemProcessorParameter(CMMNWorkflowDefinition workflowDefinition, CMMNWorkflowInstance workflowInstance, CMMNWorkflowElementInstance workflowElementInstance)
        {
            WorkflowDefinition = workflowDefinition;
            WorkflowInstance = workflowInstance;
            WorkflowElementInstance = workflowElementInstance;
        }

        public CMMNWorkflowDefinition WorkflowDefinition { get; set; }
        public CMMNWorkflowInstance WorkflowInstance { get; set; }
        public CMMNWorkflowElementInstance WorkflowElementInstance { get; set; }
    }
}
