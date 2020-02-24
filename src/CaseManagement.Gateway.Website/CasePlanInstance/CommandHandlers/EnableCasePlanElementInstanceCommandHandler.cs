using CaseManagement.Gateway.Website.CasePlanInstance.Commands;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.CommandHandlers
{
    public class EnableCasePlanElementInstanceCommandHandler : IEnableCasePlanElementInstanceCommandHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public EnableCasePlanElementInstanceCommandHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task Handle(EnableCasePlanElementInstanceCommand command)
        {
            return _casePlanInstanceService.Enable(command.CasePlanInstanceId, command.CasePlanElementInstanceId);
        }
    }
}
