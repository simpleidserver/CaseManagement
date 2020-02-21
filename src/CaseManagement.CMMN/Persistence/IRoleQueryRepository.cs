using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IRoleQueryRepository
    {
        Task<IEnumerable<RoleAggregate>> FindRolesByUser(string userId);
        Task<IEnumerable<RoleAggregate>> FindRoles(ICollection<string> roles);
    }
}
