namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class CloseCommand
    {
        public CloseCommand(string caseInstanceId)
        {
            CaseInstanceId = caseInstanceId;
        }

        public string CaseInstanceId { get; set; }
    }
}
