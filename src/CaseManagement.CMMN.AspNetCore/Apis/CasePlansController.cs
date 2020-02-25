using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.CasePlan;
using CaseManagement.CMMN.CasePlan.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Count()
        {
            var result = await _casePlanService.Count();
            return new OkObjectResult(result);
        }

        [HttpGet("me/{id}")]
        [Authorize("me_get_caseplan")]
        public async Task<IActionResult> GetMe(string id)
        {
            try
            {
                var result = await _casePlanService.GetMe(id, this.GetNameIdentifier());
                return new OkObjectResult(result);
            }
            catch(UnknownCasePlanException)
            {
                return new NotFoundResult();
            }
            catch(UnauthorizedCasePlanException)
            {
                return new UnauthorizedResult();
            }
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _casePlanService.Get(id);
                return new OkObjectResult(result);
            }
            catch (UnknownCasePlanException)
            {
                return new NotFoundResult();
            }
        }

        [HttpGet("search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _casePlanService.Search(query);
            return new OkObjectResult(result);
        }
    }
}
