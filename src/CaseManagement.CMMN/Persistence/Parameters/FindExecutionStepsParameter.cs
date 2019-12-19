namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindExecutionStepsParameter : BaseFindParameter
    {
        public FindExecutionStepsParameter() : base()
        {
            OrderBy = "start_datetime";
            Order = FindOrders.DESC;
        }

        public string ProcessFlowInstanceId { get; set; }
    }
}
