namespace CaseManagement.CMMN.Infrastructures.Bus.LaunchCasePlanInstance
{
    public class LaunchCasePlanInstanceCommand
    {
        public LaunchCasePlanInstanceCommand(string casePlanInstanceId)
        {
            CasePlanInstanceId = casePlanInstanceId;
        }

        public string CasePlanInstanceId { get; set; }
    }
}
