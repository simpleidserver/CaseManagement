namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class TerminateCommand
    {
        public TerminateCommand(string caseInstanceId, string caseInstanceElementId)
        {
            CaseInstanceId = caseInstanceId;
            CaseInstanceElementId = caseInstanceElementId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseInstanceElementId { get; set; }
        public string Performer { get; set; }
        public bool BypassUserValidation { get; set; }
    }
}
