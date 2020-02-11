using CaseManagement.Gateway.Website.Performance.DTOs;
using CaseManagement.Gateway.Website.Performance.Queries;
using CaseManagement.Gateway.Website.Performance.Services;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Performance.QueryHandlers
{
    public class SearchPerformanceQueryHandler : ISearchPerformanceQueryHandler
    {
        private readonly IPerformanceService _performanceService;

        public SearchPerformanceQueryHandler(IPerformanceService performanceService)
        {
            _performanceService = performanceService;
        }

        public Task<FindPerformanceResponse> Handle(SearchPerformanceQuery searchPerformanceQuery)
        {
            return _performanceService.Search(searchPerformanceQuery.Queries);
        }
    }
}
