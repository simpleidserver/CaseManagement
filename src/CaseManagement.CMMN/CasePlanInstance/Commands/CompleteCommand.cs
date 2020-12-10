using MediatR;
using Newtonsoft.Json.Linq;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class CompleteCommand : IRequest<bool>
    {
        public CompleteCommand(string caseInstanceId, string caseInstanceElementId)
        {
            CaseInstanceId = caseInstanceId;
            CaseInstanceElementId = caseInstanceElementId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseInstanceElementId { get; set; }
        public JObject Content { get; set; }
    }
}
