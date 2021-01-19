using MediatR;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class ReenableCommand : IRequest<bool>
    {
        public ReenableCommand(string casePlanInstanceId, string casePlanElementInstanceId)
        {
            CasePlanInstanceId = casePlanInstanceId;
            CasePlanElementInstanceId = casePlanElementInstanceId;
        }

        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
