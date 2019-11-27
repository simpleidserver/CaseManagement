namespace CaseManagement.Workflow.Persistence.Parameters
{
    public class FindWorkflowInstanceParameter : BaseFindParameter
    {
        public FindWorkflowInstanceParameter() : base()
        {

        }

        public string ProcessFlowTemplateId { get; set; }
    }
}
