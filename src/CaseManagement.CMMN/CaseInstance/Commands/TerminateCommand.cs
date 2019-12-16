namespace CaseManagement.CMMN.CaseInstance.Commands
{
    public class TerminateCommand
    {
        public TerminateCommand(string caseInstanceId, string caseElementInstanceId)
        {
            CaseInstanceId = caseInstanceId;
            CaseElementInstanceId = caseElementInstanceId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseElementInstanceId { get; set; }
    }
}
