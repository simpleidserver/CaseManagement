using CaseManagement.CMMN.CasePlanInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public interface IConfirmFormCommandHandler
    {
        Task Handle(ConfirmFormCommand confirmFormCommand);
    }
}
