using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.CasePlans.Queries;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public interface ISearchCasePlanInstanceQueryHandler
    {
        Task<FindResponse<CasePlanInstanceResponse>> Handle(SearchCasePlanInstanceQuery searchCasePlanInstanceQuery);
    }
}
