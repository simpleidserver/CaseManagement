namespace CaseManagement.CMMN.Infrastructures.Bus.LaunchProcess
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
