namespace CaseManagement.Gateway.Website.CasePlans.Commands
{
    public class TerminateCasePlanInstanceCommand
    {
        public TerminateCasePlanInstanceCommand(string casePlanInstanceId, string identityToken)
        {
            CasePlanInstanceId = casePlanInstanceId;
            IdentityToken = identityToken;
        }

        public string CasePlanInstanceId { get; set; }
        public string IdentityToken { get; set; }
    }
}
