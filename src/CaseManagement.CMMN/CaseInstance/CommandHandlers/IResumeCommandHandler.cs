using CaseManagement.CMMN.CaseInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public interface IResumeCommandHandler
    {
        Task Handle(ResumeCommand resumeCommand);
    }
}
