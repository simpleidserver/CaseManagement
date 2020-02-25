using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.DailyStatistic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Controllers
{
    [Route(CMMNConstants.RouteNames.Statistics)]
    public class StatisticsController : Controller
    {
        private readonly IDailyStatisticService _dailyStatisticService;

        public StatisticsController(IDailyStatisticService dailyStatisticService)
        {
            _dailyStatisticService = dailyStatisticService;
        }

        [HttpGet]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Get()
        {
            var result = await _dailyStatisticService.Get();
            return new OkObjectResult(result);
        }

        [HttpGet("search")]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _dailyStatisticService.Search(query);
            return new OkObjectResult(result);
        }

    }
}