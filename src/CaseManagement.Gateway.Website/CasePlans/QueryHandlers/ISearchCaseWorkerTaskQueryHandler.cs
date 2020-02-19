using CaseManagement.Gateway.Website.CasePlans.Queries;
using CaseManagement.Gateway.Website.CaseWorkerTask.DTOs;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public interface ISearchCaseWorkerTaskQueryHandler
    {
        Task<FindResponse<CaseWorkerTaskResponse>> Handle(SearchCaseWorkerTaskQuery query);
    }
}
