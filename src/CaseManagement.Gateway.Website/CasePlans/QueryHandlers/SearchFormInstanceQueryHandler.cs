using CaseManagement.Gateway.Website.CasePlans.Queries;
using CaseManagement.Gateway.Website.Extensions;
using CaseManagement.Gateway.Website.FormInstance.DTOs;
using CaseManagement.Gateway.Website.FormInstance.Services;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public class SearchFormInstanceQueryHandler : ISearchFormInstanceQueryHandler
    {
        private readonly IFormInstanceService _formInstanceService;

        public SearchFormInstanceQueryHandler(IFormInstanceService formInstanceService)
        {
            _formInstanceService = formInstanceService;
        }

        public Task<FindResponse<FormInstanceResponse>> Handle(SearchFormInstanceQuery query)
        {
            var queries = query.Queries.ToList();
            queries.TryReplace("case_plan_id", query.CasePlanId);
            return _formInstanceService.Search(queries);
        }
    }
}
