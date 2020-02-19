using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.CommandHandlers
{
    public class LaunchCasePlanInstanceCommandHandler : ILaunchCasePlanInstanceCommandHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public LaunchCasePlanInstanceCommandHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public async Task<CasePlanInstanceResponse> Handle(string casePlanId, string identityToken)
        {
            var result = await _casePlanInstanceService.AddMe(new AddCasePlanInstanceParameter
            {
                CasePlanId = casePlanId
            }, identityToken);
            await _casePlanInstanceService.LaunchMe(result.Id, identityToken);
            return result;
        }
    }
}
