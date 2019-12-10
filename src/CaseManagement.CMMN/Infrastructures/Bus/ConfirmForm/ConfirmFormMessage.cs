using System.Collections.Generic;

namespace CaseManagement.CMMN.Infrastructures.Bus.ConfirmForm
{
    public class ConfirmFormMessage
    {
        public ConfirmFormMessage(string processFlowId, string processElementId, Dictionary<string, string> content)
        {
            ProcessFlowId = processFlowId;
            ProcessElementId = processElementId;
            Content = content;
        }

        public string ProcessFlowId { get; set; }
        public string ProcessElementId { get; set; }
        public Dictionary<string, string> Content { get; set; }
    }
}
