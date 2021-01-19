using MediatR;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class ResumeCommand : IRequest<bool>
    {
        public ResumeCommand(string casePlanInstanceId, string casePlanInstanceElementId)
        {
            CasePlanInstanceId = casePlanInstanceId;
            CasePlanInstanceElementId = casePlanInstanceElementId;
        }

        public string CasePlanInstanceId { get; set; }
        public string CasePlanInstanceElementId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
