namespace CaseManagement.CMMN.Infrastructures.Bus.ReactivateProcess
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
