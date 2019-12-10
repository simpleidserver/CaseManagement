using CaseManagement.Workflow.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence
{
    public interface IRoleQueryRepository
    {
        Task<IEnumerable<RoleAggregate>> FindRolesByUser(string userId);
    }
}
