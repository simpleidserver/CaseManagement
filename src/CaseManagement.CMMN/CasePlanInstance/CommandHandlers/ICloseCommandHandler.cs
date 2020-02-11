using CaseManagement.CMMN.CasePlanInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.CommandHandlers
{
    public interface ICloseCommandHandler
    {
        Task Handle(CloseCommand closeCommand);
    }
}
