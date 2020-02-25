using CaseManagement.CMMN.Performance;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Controllers
{
    [RoutePrefix(CMMNConstants.RouteNames.Performances)]
    public class PerformanceController : ApiController
    {
        private readonly IPerformanceService _performanceService;

        public PerformanceController(IPerformanceService performanceService)
        {
            _performanceService = performanceService;
        }


        [HttpGet]
        public async Task<IHttpActionResult> GetPerformances()
        {
            var result = await _performanceService.GetPerformances();
            return Ok(result);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> SearchPerformances()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _performanceService.SearchPerformances(query);
            return Ok(result);
        }
    }
}
