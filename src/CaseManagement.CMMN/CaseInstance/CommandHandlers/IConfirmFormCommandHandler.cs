using CaseManagement.CMMN.CaseInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public interface IConfirmFormCommandHandler
    {
        Task<bool> Handle(ConfirmFormCommand confirmFormCommand);
    }
}
