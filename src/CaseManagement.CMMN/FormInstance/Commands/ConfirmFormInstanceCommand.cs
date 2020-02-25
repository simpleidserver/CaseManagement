using Newtonsoft.Json.Linq;

namespace CaseManagement.CMMN.FormInstance.Commands
{
    public class ConfirmFormInstanceCommand
    {
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public JObject Content { get; set; }
        public string Performer { get; set; }
        public bool BypassUserValidation { get; set; }
    }
}
