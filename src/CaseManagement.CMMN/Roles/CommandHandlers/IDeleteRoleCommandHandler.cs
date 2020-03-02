using CaseManagement.CMMN.Roles.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Roles.CommandHandlers
{
    public interface IDeleteRoleCommandHandler
    {
        Task Handle(DeleteRoleCommand updateRoleCommand);
    }
}
