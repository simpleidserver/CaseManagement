using CaseManagement.Gateway.Website.Role.Commands;
using CaseManagement.Gateway.Website.Role.DTOs;
using CaseManagement.Gateway.Website.Role.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Role.CommandHandlers
{
    public class AddRoleCommandHandler : IAddRoleCommandHandler
    {
        private readonly IRoleService _roleService;

        public AddRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public Task<RoleResponse> Handle(AddRoleCommand command)
        {
            return _roleService.Add(new AddRoleParameter { Role = command.Role });
        }
    }
}
