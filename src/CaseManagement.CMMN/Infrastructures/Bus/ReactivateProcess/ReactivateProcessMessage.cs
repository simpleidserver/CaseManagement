namespace CaseManagement.CMMN.Infrastructures.Bus.LaunchProcess
{
    public class ReactivateProcessMessage
    {
        public ReactivateProcessMessage(string id, string caseInstanceId)
        {
            Id = id;
            CaseInstanceId = caseInstanceId;
        }
        
        public string Id { get; set; }
        public string CaseInstanceId { get; set; }
    }
}
