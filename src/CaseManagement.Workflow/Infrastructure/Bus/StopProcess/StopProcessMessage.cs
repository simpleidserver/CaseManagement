namespace CaseManagement.Workflow.Infrastructure.Bus.StopProcess
{
    public class StopProcessMessage
    {
        public StopProcessMessage (string processFlowId)
        {
            ProcessFlowId = processFlowId;
        }

        public string ProcessFlowId { get; set; }
    }
}
