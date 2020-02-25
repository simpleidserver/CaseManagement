using CaseManagement.CMMN.CaseWorkerTask.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseWorkerTask.CommandHandlers
{
    public interface IConfirmCaseWorkerTaskHandler
    {
        Task Handle(ConfirmCaseWorkerTask command);
    }
}
