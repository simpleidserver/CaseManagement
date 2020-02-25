using CaseManagement.CMMN.DailyStatistic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Controllers
{
    [RoutePrefix(CMMNConstants.RouteNames.Statistics)]
    public class StatisticsController : ApiController
    {
        private readonly IDailyStatisticService _dailyStatisticService;

        public StatisticsController(IDailyStatisticService dailyStatisticService)
        {
            _dailyStatisticService = dailyStatisticService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var result = await _dailyStatisticService.Get();
            return Ok(result);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _dailyStatisticService.Search(query);
            return Ok(result);
        }
    }
}