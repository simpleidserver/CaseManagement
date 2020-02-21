using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.CasePlanInstance.Queries;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers
{
    public interface ISearchAssignedCasePlanInstanceQueryHandler
    {
        Task<FindResponse<CasePlanInstanceResponse>> Handle(SearchAssignedCasePlanInstanceQuery query);
    }
}
