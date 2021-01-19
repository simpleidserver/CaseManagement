using MediatR;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class SuspendCommand : IRequest<bool>
    {
        public SuspendCommand(string casePlanInstanceId, string casePlanInstanceElementId)
        {
            CasePlanInstanceId = casePlanInstanceId;
            CasePlanInstanceElementId = casePlanInstanceElementId;
        }

        public string CasePlanInstanceId { get; set; }
        public string CasePlanInstanceElementId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
