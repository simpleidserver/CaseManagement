using CaseManagement.Gateway.Website.Role.Commands;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Role.CommandHandlers
{
    public interface IDeleteRoleCommandHandler
    {
        Task Handle(DeleteRoleCommand cmd);
    }
}
