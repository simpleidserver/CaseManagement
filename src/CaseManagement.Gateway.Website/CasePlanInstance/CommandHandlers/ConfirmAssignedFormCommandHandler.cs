using CaseManagement.Gateway.Website.CasePlanInstance.Commands;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.CommandHandlers
{
    public class ConfirmAssignedFormCommandHandler : IConfirmAssignedFormCommandHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public ConfirmAssignedFormCommandHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task Handle(ConfirmAssignedFormCommand confirmFormCommand)
        {
            return _casePlanInstanceService.ConfirmFormMe(confirmFormCommand.CasePlanInstanceId, confirmFormCommand.CasePlanElementInstanceId, confirmFormCommand.Content, confirmFormCommand.IdentityToken);
        }
    }
}
