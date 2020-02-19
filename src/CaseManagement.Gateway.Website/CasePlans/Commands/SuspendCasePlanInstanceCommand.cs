namespace CaseManagement.Gateway.Website.CasePlans.Commands
{
    public class SuspendCasePlanInstanceCommand
    {
        public SuspendCasePlanInstanceCommand(string casePlanInstanceId, string identityToken)
        {
            CasePlanInstanceId = casePlanInstanceId;
            IdentityToken = identityToken;
        }

        public string CasePlanInstanceId { get; set; }
        public string IdentityToken { get; set; }
    }
}
