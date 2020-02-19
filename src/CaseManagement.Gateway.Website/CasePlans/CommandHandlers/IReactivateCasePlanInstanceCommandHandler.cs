using CaseManagement.Gateway.Website.CasePlans.Commands;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.CommandHandlers
{
    public interface IReactivateCasePlanInstanceCommandHandler
    {
        Task Handle(ReactivateCasePlanInstanceCommand cmd);
    }
}
