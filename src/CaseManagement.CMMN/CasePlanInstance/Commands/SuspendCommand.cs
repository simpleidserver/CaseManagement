namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class SuspendCommand
    {
        public SuspendCommand(string caseInstanceId, string caseInstanceElementId)
        {
            CaseInstanceId = caseInstanceId;
            CaseInstanceElementId = caseInstanceElementId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseInstanceElementId { get; set; }
    }
}
