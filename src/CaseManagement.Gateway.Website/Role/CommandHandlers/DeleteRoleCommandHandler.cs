using CaseManagement.Gateway.Website.Role.Commands;
using CaseManagement.Gateway.Website.Role.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Role.CommandHandlers
{
    public class DeleteRoleCommandHandler : IDeleteRoleCommandHandler
    {
        private readonly IRoleService _roleService;

        public DeleteRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public Task Handle(DeleteRoleCommand cmd)
        {
            return _roleService.Delete(cmd.Role);
        }
    }
}
