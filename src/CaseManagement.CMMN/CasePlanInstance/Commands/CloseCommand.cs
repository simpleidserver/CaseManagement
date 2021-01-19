using MediatR;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class CloseCommand : IRequest<bool>
    {
        public CloseCommand(string casePlanInstanceId)
        {
            CasePlanInstanceId = casePlanInstanceId;
        }

        public string CasePlanInstanceId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
