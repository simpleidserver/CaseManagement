using CaseManagement.CMMN.CasePlanInstance.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public interface ITerminateCommandHandler
    {
        Task Handle(TerminateCommand terminateCommand, CancellationToken cancellationToken);
    }
}
