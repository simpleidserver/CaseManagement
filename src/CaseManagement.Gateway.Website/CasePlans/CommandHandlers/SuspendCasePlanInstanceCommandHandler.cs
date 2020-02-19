using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using CaseManagement.Gateway.Website.CasePlans.Commands;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.CommandHandlers
{
    public class SuspendCasePlanInstanceCommandHandler : ISuspendCasePlanInstanceCommandHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public SuspendCasePlanInstanceCommandHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task Handle(SuspendCasePlanInstanceCommand cmd)
        {
            return _casePlanInstanceService.SuspendMe(cmd.CasePlanInstanceId, cmd.IdentityToken);
        }
    }
}
