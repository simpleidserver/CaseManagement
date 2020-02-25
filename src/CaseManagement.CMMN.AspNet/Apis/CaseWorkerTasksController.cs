using CaseManagement.CMMN.CaseWorkerTask;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Apis
{
    [RoutePrefix(CMMNConstants.RouteNames.CaseWorkerTasks)]
    public class CaseWorkerTasksController : ApiController
    {
        private readonly ICaseWorkerTaskService _caseWorkerTaskService;

        public CaseWorkerTasksController(ICaseWorkerTaskService caseWorkerTaskService)
        {
            _caseWorkerTaskService = caseWorkerTaskService;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _caseWorkerTaskService.Search(query);
            return Ok(result);
        }
    }
}