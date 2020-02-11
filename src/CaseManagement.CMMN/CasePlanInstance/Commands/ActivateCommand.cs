namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class ActivateCommand
    {
        public ActivateCommand(string casePlanInstanceId, string casePlanElementInstanceId)
        {
            CasePlanInstanceId = casePlanInstanceId;
            CasePlanElementInstanceId = casePlanElementInstanceId;
        }

        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public string Performer { get; set; }
    }
}
