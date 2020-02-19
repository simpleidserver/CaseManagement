namespace CaseManagement.Gateway.Website.CasePlans.Commands
{
    public class CloseCasePlanInstanceCommand
    {
        public CloseCasePlanInstanceCommand(string casePlanInstanceId, string identityToken)
        {
            CasePlanInstanceId = casePlanInstanceId;
            IdentityToken = identityToken;
        }

        public string CasePlanInstanceId { get; set; }
        public string IdentityToken { get; set; }
    }
}
