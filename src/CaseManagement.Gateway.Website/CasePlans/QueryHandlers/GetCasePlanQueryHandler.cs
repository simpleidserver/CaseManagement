using CaseManagement.Gateway.Website.CasePlans.DTOs;
using CaseManagement.Gateway.Website.CasePlans.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public class GetCasePlanQueryHandler : IGetCasePlanQueryHandler
    {
        private readonly ICasePlanService _casePlanService;

        public GetCasePlanQueryHandler(ICasePlanService casePlanService)
        {
            _casePlanService = casePlanService;
        }

        public Task<CasePlanResponse> Handle(string id, string identityToken)
        {
            return _casePlanService.GetMe(id, identityToken);
        }
    }
}
