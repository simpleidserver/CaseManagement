using CaseManagement.Gateway.Website.Performance.DTOs;
using CaseManagement.Gateway.Website.Performance.Queries;
using CaseManagement.Gateway.Website.Performance.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Performance.QueryHandlers
{
    public class GetPerformanceQueryHandler : IGetPerformanceQueryHandler
    {
        private readonly IPerformanceService _performanceService;

        public GetPerformanceQueryHandler(IPerformanceService performanceService)
        {
            _performanceService = performanceService;
        }

        public Task<IEnumerable<string>> Handle(GetPerformanceQuery getPerformanceQuery)
        {
            return _performanceService.Get();
        }
    }
}
