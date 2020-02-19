namespace CaseManagement.Gateway.Website.CasePlans.Commands
{
    public class ReactivateCasePlanInstanceCommand
    {
        public ReactivateCasePlanInstanceCommand(string casePlanInstanceId, string identityToken)
        {
            CasePlanInstanceId = casePlanInstanceId;
            IdentityToken = identityToken;
        }

        public string CasePlanInstanceId { get; set; }
        public string IdentityToken { get; set; }
    }
}
