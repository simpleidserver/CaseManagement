namespace CaseManagement.Gateway.Website.CasePlanInstance.Commands
{
    public class EnableAssignedCasePlanElementInstanceCommand
    {
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public string IdentityToken { get; set; }
    }
}
