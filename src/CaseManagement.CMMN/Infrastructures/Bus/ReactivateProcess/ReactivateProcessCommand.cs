namespace CaseManagement.CMMN.Infrastructures.Bus.ReactivateProcess
{
    public class ReactivateProcessCommand
    {
        public ReactivateProcessCommand(string caseInstanceId)
        {
            CasePlanInstanceId = caseInstanceId;
        }
        
        public string CasePlanInstanceId { get; set; }
    }
}
