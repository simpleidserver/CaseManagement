using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryRoleQueryRepository : IRoleQueryRepository
    {
        private readonly ICollection<RoleAggregate> _roles;

        public InMemoryRoleQueryRepository(ICollection<RoleAggregate> roles)
        {
            _roles = roles;
        }

        public Task<IEnumerable<RoleAggregate>> FindRolesByUser(string userId)
        {
            return Task.FromResult(_roles.Where(r => r.UserIds.Contains(userId)));
        }
    }
}
