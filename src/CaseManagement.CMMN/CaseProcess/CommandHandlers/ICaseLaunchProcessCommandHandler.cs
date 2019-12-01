using CaseManagement.CMMN.CaseProcess.Commands;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseProcess.CommandHandlers
{
    public interface ICaseLaunchProcessCommandHandler
    {
        Task<CaseProcessResponse> Handle(LaunchCaseProcessCommand launchCaseProcessCommand);
    }
}
