using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.Performance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.Performances)]
    public class PerformanceController : Controller
    {
        private readonly IPerformanceService _performanceService;

        public PerformanceController(IPerformanceService performanceService)
        {
            _performanceService = performanceService;
        }

        [HttpGet]
        [Authorize("get_performance")]
        public async Task<IActionResult> GetPerformances()
        {
            var result = await _performanceService.GetPerformances();
            return new OkObjectResult(result);
        }

        [HttpGet("search")]
        [Authorize("get_performance")]
        public async Task<IActionResult> SearchPerformances()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _performanceService.SearchPerformances(query);
            return new OkObjectResult(result);
        }
    }
}
