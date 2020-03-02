using System.Threading.Tasks;
using CaseManagement.Gateway.Website.Role.DTOs;
using CaseManagement.Gateway.Website.Role.Queries;
using CaseManagement.Gateway.Website.Role.Services;

namespace CaseManagement.Gateway.Website.Role.QueryHandlers
{
    public class GetRoleQueryHandler : IGetRoleQueryHandler
    {
        private readonly IRoleService _roleService;

        public GetRoleQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public Task<RoleResponse> Handle(GetRoleQuery query)
        {
            return _roleService.Get(query.Role);
        }
    }
}
