using CaseManagement.Gateway.Website.CasePlans.Queries;
using CaseManagement.Gateway.Website.FormInstance.DTOs;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.QueryHandlers
{
    public interface ISearchFormInstanceQueryHandler
    {
        Task<FindResponse<FormInstanceResponse>> Handle(SearchFormInstanceQuery query);
    }
}