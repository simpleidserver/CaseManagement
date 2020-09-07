using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.CasePlan;
using CaseManagement.CMMN.CasePlan.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CasePlans)]
    public class CasePlansController : Controller
    {
        private readonly ICasePlanService _casePlanService;

        public CasePlansController(ICasePlanService casePlanService)
        {
            _casePlanService = casePlanService;
        }

        [HttpGet("count")]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Count(CancellationToken token)
        {
            var result = await _casePlanService.Count(token);
            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> Get(string id, CancellationToken token)
        {
            try
            {
                var result = await _casePlanService.Get(id, token);
                return new OkObjectResult(result);
            }
            catch (UnknownCasePlanException)
            {
                return new NotFoundResult();
            }
        }

        [HttpGet("search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> Search(CancellationToken token)
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _casePlanService.Search(query, token);
            return new OkObjectResult(result);
        }
    }
}
