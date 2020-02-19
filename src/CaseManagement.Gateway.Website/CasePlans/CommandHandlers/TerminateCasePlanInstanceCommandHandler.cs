using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using CaseManagement.Gateway.Website.CasePlans.Commands;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.CommandHandlers
{
    public class TerminateCasePlanInstanceCommandHandler : ITerminateCasePlanInstanceCommandHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public TerminateCasePlanInstanceCommandHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task Handle(TerminateCasePlanInstanceCommand cmd)
        {
            return _casePlanInstanceService.CloseMe(cmd.CasePlanInstanceId, cmd.IdentityToken);
        }
    }
}
