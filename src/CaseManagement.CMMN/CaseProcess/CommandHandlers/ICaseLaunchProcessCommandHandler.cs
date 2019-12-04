using CaseManagement.CMMN.CaseProcess.Commands;
using CaseManagement.CMMN.CaseProcess.ProcessHandlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseProcess.CommandHandlers
{
    public interface ICaseLaunchProcessCommandHandler
    {
        Task Handle(LaunchCaseProcessCommand launchCaseProcessCommand, Func<CaseProcessResponse, Task> callback, CancellationToken token);
    }
}
