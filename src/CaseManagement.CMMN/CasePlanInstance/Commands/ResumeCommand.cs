namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class ResumeCommand
    {
        public ResumeCommand(string caseInstanceId, string caseInstanceElementId)
        {
            CaseInstanceId = caseInstanceId;
            CaseInstanceElementId = caseInstanceElementId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseInstanceElementId { get; set; }
    }
}
