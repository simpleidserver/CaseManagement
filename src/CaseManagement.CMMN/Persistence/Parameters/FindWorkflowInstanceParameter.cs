namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindWorkflowInstanceParameter : BaseFindParameter
    {
        public FindWorkflowInstanceParameter() : base()
        {

        }

        public string CaseDefinitionId { get; set; }
    }
}