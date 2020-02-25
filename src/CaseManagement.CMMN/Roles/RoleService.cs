using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRoleQueryRepository _roleQueryRepository;

        public RoleService(IRoleQueryRepository roleQueryRepository)
        {
            _roleQueryRepository = roleQueryRepository;
        }

        public Task<IEnumerable<RoleAggregate>> FindRolesByUser(string user)
        {
            return _roleQueryRepository.FindRolesByUser(user);
        }
    }
}
