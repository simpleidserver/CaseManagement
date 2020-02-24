using CaseManagement.Gateway.Website.CasePlanInstance.Commands;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.CommandHandlers
{
    public interface IEnableCasePlanElementInstanceCommandHandler
    {
        Task Handle(EnableCasePlanElementInstanceCommand command);
    }
}
