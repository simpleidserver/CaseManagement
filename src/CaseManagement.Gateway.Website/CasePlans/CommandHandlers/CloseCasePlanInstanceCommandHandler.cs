using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using CaseManagement.Gateway.Website.CasePlans.Commands;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.CommandHandlers
{
    public class CloseCasePlanInstanceCommandHandler : ICloseCasePlanInstanceCommandHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public CloseCasePlanInstanceCommandHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task Handle(CloseCasePlanInstanceCommand cmd)
        {
            return _casePlanInstanceService.CloseMe(cmd.CasePlanInstanceId, cmd.IdentityToken);
        }
    }
}
