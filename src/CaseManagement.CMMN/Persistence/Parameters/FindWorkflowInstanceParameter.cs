namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindWorkflowInstanceParameter : BaseFindParameter
    {
        public FindWorkflowInstanceParameter() : base()
        {

        }

        public string CasePlanId { get; set; }
        public string CaseOwner { get; set; }
    }
}