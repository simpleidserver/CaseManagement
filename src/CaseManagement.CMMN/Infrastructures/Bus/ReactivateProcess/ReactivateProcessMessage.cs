namespace CaseManagement.CMMN.Infrastructures.Bus.LaunchProcess
{
    public class ReactivateProcessMessage
    {
        public ReactivateProcessMessage(string caseInstanceId)
        {
            CaseInstanceId = caseInstanceId;
        }
        
        public string CaseInstanceId { get; set; }
    }
}
