using CaseManagement.Gateway.Website.Performance.DTOs;
using CaseManagement.Gateway.Website.Performance.Queries;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Performance.QueryHandlers
{
    public interface ISearchPerformanceQueryHandler
    {
        Task<FindPerformanceResponse> Handle(SearchPerformanceQuery searchPerformanceQuery);
    }
}
