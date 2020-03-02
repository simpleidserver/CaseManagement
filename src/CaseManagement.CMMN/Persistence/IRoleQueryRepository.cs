using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IRoleQueryRepository
    {
        Task<RoleAggregate> FindById(string id);
        Task<IEnumerable<RoleAggregate>> FindRolesByUser(string userId);
        Task<IEnumerable<RoleAggregate>> FindRoles(ICollection<string> roles);
        Task<FindResponse<RoleAggregate>> Search(FindRoleParameter findRoleParameter);
    }
}
