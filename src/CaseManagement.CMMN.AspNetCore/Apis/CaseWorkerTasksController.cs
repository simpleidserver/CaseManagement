using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.CaseWorkerTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CaseWorkerTasks)]
    public class CaseWorkerTasksController : Controller
    {
        private readonly ICaseWorkerTaskService _caseWorkerTaskService;

        public CaseWorkerTasksController(ICaseWorkerTaskService caseWorkerTaskService)
        {
            _caseWorkerTaskService = caseWorkerTaskService;
        }

        [HttpGet("search")]
        [Authorize("get_caseworkertasks")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _caseWorkerTaskService.Search(query);
            return new OkObjectResult(result);
        }
    }
}