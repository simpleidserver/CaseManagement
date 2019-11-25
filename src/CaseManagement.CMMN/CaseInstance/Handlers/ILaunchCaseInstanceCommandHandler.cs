using CaseManagement.CMMN.CaseInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Handlers
{
    public interface ILaunchCaseInstanceCommandHandler
    {
        Task Handle(LaunchCaseInstanceCommand launchCaseInstanceCommand);
    }
}
