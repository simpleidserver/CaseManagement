using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.CasePlanInstance.Queries;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers
{
    public class GetCasePlanInstanceQueryHandler : IGetCasePlanInstanceQueryHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public GetCasePlanInstanceQueryHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task<CasePlanInstanceResponse> Handle(GetCasePlanInstanceQuery query)
        {
            return _casePlanInstanceService.Get(query.CasePlanInstanceId);
        }
    }
}
