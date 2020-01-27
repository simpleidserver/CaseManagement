using CaseManagement.CMMN.CaseInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public interface IConfirmTableItemCommandHandler
    {
        Task Handle(ConfirmTableItemCommand confirmPlanItemCommand);
    }
}
