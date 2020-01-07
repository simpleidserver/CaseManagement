using CaseManagement.CMMN.CaseInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.CommandHandlers
{
    public interface IActivateCommandHandler
    {
        Task Handle(ActivateCommand command);
    }
}
