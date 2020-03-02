using CaseManagement.Gateway.Website.Role.Commands;
using CaseManagement.Gateway.Website.Role.DTOs;
using CaseManagement.Gateway.Website.Role.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Role.CommandHandlers
{
    public class UpdateRoleCommandHandler : IUpdateRoleCommandHandler
    {
        private readonly IRoleService _roleService;

        public UpdateRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public Task Handle(UpdateRoleCommand command)
        {
            return _roleService.Update(command.Role, new UpdateRoleParameter { Users = command.Users });
        }
    }
}
