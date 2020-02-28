using CaseManagement.Gateway.Website.CasePlanInstance.Commands;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.CommandHandlers
{
    public class EnableAssignedCasePlanElementInstanceCommandHandler : IEnableAssignedCasePlanElementInstanceCommandHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public EnableAssignedCasePlanElementInstanceCommandHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task Handle(EnableAssignedCasePlanElementInstanceCommand command)
        {
            return _casePlanInstanceService.EnableMe(command.CasePlanInstanceId, command.CasePlanElementInstanceId, command.IdentityToken);
        }
    }
}
