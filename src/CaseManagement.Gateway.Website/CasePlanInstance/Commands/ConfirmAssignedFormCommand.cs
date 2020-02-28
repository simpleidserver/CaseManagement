using Newtonsoft.Json.Linq;

namespace CaseManagement.Gateway.Website.CasePlanInstance.Commands
{
    public class ConfirmAssignedFormCommand
    {
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public JObject Content { get; set; }
        public string IdentityToken { get; set; }
    }
}
