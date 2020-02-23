using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.CasePlanInstance.Queries;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers
{
    public class GetAssignedCasePlanInstanceQueryHandler : IGetAssignedCasePlanInstanceQueryHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public GetAssignedCasePlanInstanceQueryHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task<CasePlanInstanceResponse> Handle(GetAssignedCasePlanInstanceQuery query)
        {
            return _casePlanInstanceService.GetMe(query.CasePlanInstanceId, query.IdentityToken);
        }
    }
}
