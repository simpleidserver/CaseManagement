using Newtonsoft.Json.Linq;

namespace CaseManagement.CMMN.Infrastructures.Bus.ConfirmForm
{
    public class ConfirmFormMessage
    {
        public ConfirmFormMessage(string processFlowId, string processElementId, JObject content)
        {
            ProcessFlowId = processFlowId;
            ProcessElementId = processElementId;
            Content = content;
        }

        public string ProcessFlowId { get; set; }
        public string ProcessElementId { get; set; }
        public JObject Content { get; set; }
    }
}
