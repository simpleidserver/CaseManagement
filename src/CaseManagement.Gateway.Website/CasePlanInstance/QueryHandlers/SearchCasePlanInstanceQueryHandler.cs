using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.CasePlanInstance.Queries;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers
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
            return _casePlanInstanceService.Search(queries);
        }
    }
}
