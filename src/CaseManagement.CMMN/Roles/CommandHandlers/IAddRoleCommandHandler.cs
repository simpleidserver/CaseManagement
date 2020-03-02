using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Roles.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Roles.CommandHandlers
{
    public interface IAddRoleCommandHandler
    {
        Task<RoleAggregate> Handle(AddRoleCommand addRoleCommand);
    }
}
