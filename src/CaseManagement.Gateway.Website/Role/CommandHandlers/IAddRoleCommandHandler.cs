using CaseManagement.Gateway.Website.Role.Commands;
using CaseManagement.Gateway.Website.Role.DTOs;
using CaseManagement.Gateway.Website.Role.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Role.CommandHandlers
{
    public interface IAddRoleCommandHandler
    {
        Task<RoleResponse> Handle(AddRoleCommand command);
    }
}
