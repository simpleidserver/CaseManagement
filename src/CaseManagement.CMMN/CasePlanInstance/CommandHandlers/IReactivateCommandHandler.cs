using CaseManagement.CMMN.CasePlanInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public interface IReactivateCommandHandler
    {
        Task Handle(ReactivateCommand command);
    }
}
