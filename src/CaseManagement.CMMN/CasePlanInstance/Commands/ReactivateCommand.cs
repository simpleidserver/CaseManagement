namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class ReactivateCommand
    {
        public ReactivateCommand(string caseInstanceId, string caseInstanceElementId)
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
