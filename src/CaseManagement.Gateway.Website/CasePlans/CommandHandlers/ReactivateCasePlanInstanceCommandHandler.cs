using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using CaseManagement.Gateway.Website.CasePlans.Commands;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.CommandHandlers
{
    public class ReactivateCasePlanInstanceCommandHandler : IReactivateCasePlanInstanceCommandHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public ReactivateCasePlanInstanceCommandHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task Handle(ReactivateCasePlanInstanceCommand cmd)
        {
            return _casePlanInstanceService.ReactivateMe(cmd.CasePlanInstanceId, cmd.IdentityToken);
        }
    }
}
