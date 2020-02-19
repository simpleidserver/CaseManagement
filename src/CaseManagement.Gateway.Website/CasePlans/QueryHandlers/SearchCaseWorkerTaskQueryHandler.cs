using CaseManagement.Gateway.Website.CasePlans.Queries;
using CaseManagement.Gateway.Website.CaseWorkerTask.DTOs;
using CaseManagement.Gateway.Website.CaseWorkerTask.Services;
using CaseManagement.Gateway.Website.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public class SearchCaseWorkerTaskQueryHandler : ISearchCaseWorkerTaskQueryHandler
    {
        private readonly ICaseWorkerTaskService _caseWorkerTaskService;

        public SearchCaseWorkerTaskQueryHandler(ICaseWorkerTaskService caseWorkerTaskService)
        {
            _caseWorkerTaskService = caseWorkerTaskService;
        }

        public Task<FindResponse<CaseWorkerTaskResponse>> Handle(SearchCaseWorkerTaskQuery query)
        {
            var queries = query.Queries.ToList();
            queries.TryReplace("case_plan_id", query.CasePlanId);
            return _caseWorkerTaskService.Search(queries);
        }
    }
}
