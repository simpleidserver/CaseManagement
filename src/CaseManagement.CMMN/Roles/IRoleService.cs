using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Roles
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleAggregate>> FindRolesByUser(string user);
    }
}
