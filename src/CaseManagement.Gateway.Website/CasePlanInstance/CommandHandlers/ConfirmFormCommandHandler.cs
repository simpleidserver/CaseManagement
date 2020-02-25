using CaseManagement.Gateway.Website.CasePlanInstance.Commands;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.CommandHandlers
{
    public class ConfirmFormCommandHandler : IConfirmFormCommandHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public ConfirmFormCommandHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task Handle(ConfirmFormCommand confirmFormCommand)
        {
            return _casePlanInstanceService.ConfirmForm(confirmFormCommand.CasePlanInstanceId, confirmFormCommand.CasePlanElementInstanceId, confirmFormCommand.Content);
        }
    }
}
