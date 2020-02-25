namespace CaseManagement.CMMN.CaseWorkerTask.Commands
{
    public class ConfirmCaseWorkerTask
    {
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public bool BypassUserValidation { get; set; }
        public string Performer { get; set; }
    }
}
