using CaseManagement.CMMN.CasePlanInstance.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public interface ILaunchCaseInstanceCommandHandler
    {
        Task Handle(LaunchCaseInstanceCommand launchCaseInstanceCommand, CancellationToken token);
    }
}
