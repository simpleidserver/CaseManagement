using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using CaseManagement.Gateway.Website.CasePlans.Queries;
using CaseManagement.Gateway.Website.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public class SearchCasePlanInstanceQueryHandler : ISearchCasePlanInstanceQueryHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public SearchCasePlanInstanceQueryHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task<FindResponse<CasePlanInstanceResponse>> Handle(SearchCasePlanInstanceQuery searchCasePlanInstanceQuery)
        {
            var queries = searchCasePlanInstanceQuery.Queries.ToList();
            queries.TryReplace("owner", searchCasePlanInstanceQuery.Owner);
            queries.TryReplace("case_plan_id", searchCasePlanInstanceQuery.CasePlanId);
            return _casePlanInstanceService.Search(queries);
        }
    }
}
