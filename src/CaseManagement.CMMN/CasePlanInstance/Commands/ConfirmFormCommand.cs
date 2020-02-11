using Newtonsoft.Json.Linq;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class ConfirmFormCommand
    {
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public JObject Content { get; set; }
        public string Performer { get; set; }
    }
}
