using MediatR;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class UpdatePermissionsRoleCommand : IRequest<bool>
    {
        public string CasePlanInstanceId { get; set; }
        public string RoleId { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; }
    }
}
