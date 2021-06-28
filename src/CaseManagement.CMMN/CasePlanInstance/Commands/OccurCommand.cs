using MediatR;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class OccurCommand : IRequest<bool>
    {
        public OccurCommand(string casePlanInstanceId, string casePlanElementInstanceId)
        {
            CasePlanInstanceId = casePlanInstanceId;
            CasePlanElementInstanceId = casePlanElementInstanceId;
        }

        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
