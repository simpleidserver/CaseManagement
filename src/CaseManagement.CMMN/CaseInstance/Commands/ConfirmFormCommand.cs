using Newtonsoft.Json.Linq;

namespace CaseManagement.CMMN.CaseInstance.Commands
{
    public class ConfirmFormCommand
    {
        public string CaseInstanceId { get; set; }
        public string CaseElementInstanceId { get; set; }
        public JObject Content { get; set; }
    }
}
