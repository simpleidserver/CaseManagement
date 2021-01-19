using MediatR;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class AddChildCommand : IRequest<bool>
    {
        public AddChildCommand(string id, string elt)
        {
            CasePlanInstanceId = id;
            CasePlanInstanceElementId = elt;
        }

        public string CasePlanInstanceId { get; set; }
        public string CasePlanInstanceElementId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
