namespace CaseManagement.CMMN.CaseInstance.Commands
{
    public class ActivateCommand
    {
        public ActivateCommand(string caseInstanceId, string caseElementInstanceId)
        {
            CaseInstanceId = caseInstanceId;
            CaseElementInstanceId = caseElementInstanceId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseElementInstanceId { get; set; }
    }
}
