using CaseManagement.Gateway.Website.CasePlans.DTOs;
using CaseManagement.Gateway.Website.CasePlans.Queries;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public interface ISearchCasePlanHistoryQueryHandler
    {
        Task<FindResponse<CasePlanResponse>> Handle(SearchCasePlanHistoryQuery query);
    }
}
