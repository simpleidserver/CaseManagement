using CaseManagement.Gateway.Website.CasePlans.DTOs;
using CaseManagement.Gateway.Website.CasePlans.Queries;
using CaseManagement.Gateway.Website.CasePlans.Services;
using CaseManagement.Gateway.Website.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public class SearchMyLatestCasePlanQueryHandler : ISearchMyLatestCasePlanQueryHandler
    {
        private readonly ICasePlanService _casePlanService;

        public SearchMyLatestCasePlanQueryHandler(ICasePlanService casePlanService)
        {
            _casePlanService = casePlanService;
        }

        public Task<FindResponse<CasePlanResponse>> Handle(SearchMyLatestCasePlanQuery searchMyLatestCasePlanQuery)
        {
            var queries = searchMyLatestCasePlanQuery.Queries.ToList();
            queries.TryReplace("take_latest", "true");
            queries.TryReplace("owner", searchMyLatestCasePlanQuery.NameIdentifier);
            return _casePlanService.Search(queries);
        }
    }
}
