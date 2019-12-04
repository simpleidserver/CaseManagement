namespace CaseManagement.Workflow.Infrastructure.Bus.LaunchProcess
{
    public class LaunchProcessMessage
    {
        public LaunchProcessMessage(string processFlowId)
        {
            ProcessFlowId = processFlowId;
        }

        public string ProcessFlowId { get; set; }
    }
}
