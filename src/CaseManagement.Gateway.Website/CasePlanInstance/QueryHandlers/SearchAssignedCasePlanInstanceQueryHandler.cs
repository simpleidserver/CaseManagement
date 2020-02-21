using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.CasePlanInstance.Queries;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers
{
    public class SearchAssignedCasePlanInstanceQueryHandler : ISearchAssignedCasePlanInstanceQueryHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;

        public SearchAssignedCasePlanInstanceQueryHandler(ICasePlanInstanceService casePlanInstanceService)
        {
            _casePlanInstanceService = casePlanInstanceService;
        }

        public Task<FindResponse<CasePlanInstanceResponse>> Handle(SearchAssignedCasePlanInstanceQuery query)
        {
            var queries = query.Queries.ToList();
            return _casePlanInstanceService.SearchMe(queries, query.IdentityToken);
        }
    }
}
