using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNWorkflowElementExecutionHistory
    {
        public CMMNWorkflowElementExecutionHistory(string workflowElementDefinitionId, DateTime startDateTime)
        {
            WorkflowElementDefinitionId = workflowElementDefinitionId;
            StartDateTime = startDateTime;
        }

        public string WorkflowElementDefinitionId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
