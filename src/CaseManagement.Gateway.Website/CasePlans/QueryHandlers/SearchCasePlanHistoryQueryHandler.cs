using CaseManagement.Gateway.Website.CasePlans.DTOs;
using CaseManagement.Gateway.Website.CasePlans.Queries;
using CaseManagement.Gateway.Website.CasePlans.Services;
using CaseManagement.Gateway.Website.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public class SearchCasePlanHistoryQueryHandler : ISearchCasePlanHistoryQueryHandler
    {
        private readonly ICasePlanService _casePlanService;

        public SearchCasePlanHistoryQueryHandler(ICasePlanService casePlanService)
        {
            _casePlanService = casePlanService;
        }

        public Task<FindResponse<CasePlanResponse>> Handle(SearchCasePlanHistoryQuery query)
        {
            var queries = query.Queries.ToList();
            queries.TryReplace("owner", query.NameIdentifier);
            queries.TryReplace("case_plan_id", query.CasePlanId);
            return _casePlanService.Search(queries);
        }
    }
}
