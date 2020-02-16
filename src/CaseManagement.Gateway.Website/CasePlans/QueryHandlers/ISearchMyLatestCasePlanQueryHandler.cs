using CaseManagement.Gateway.Website.CasePlans.DTOs;
using CaseManagement.Gateway.Website.CasePlans.Queries;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public interface ISearchMyLatestCasePlanQueryHandler
    {
        Task<FindResponse<CasePlanResponse>> Handle(SearchMyLatestCasePlanQuery searchMyLatestCasePlanQuery);
    }
}
