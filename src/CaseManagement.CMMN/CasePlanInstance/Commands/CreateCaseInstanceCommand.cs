using MediatR;
using CaseManagement.CMMN.CasePlanInstance.Results;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class CreateCaseInstanceCommand : IRequest<CasePlanInstanceResult>
    {
        public CreateCaseInstanceCommand()
        {
            Permissions = new List<UpdatePermissionsRoleCommand>();
        }

        public string CasePlanId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public ICollection<UpdatePermissionsRoleCommand> Permissions { get; set; }
    }
}