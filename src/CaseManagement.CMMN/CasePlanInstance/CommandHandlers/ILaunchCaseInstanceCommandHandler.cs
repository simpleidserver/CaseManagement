using CaseManagement.CMMN.CasePlanInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public interface ILaunchCaseInstanceCommandHandler
    {
        Task Handle(LaunchCaseInstanceCommand launchCaseInstanceCommand);
    }
}
