using CaseManagement.CMMN.AspNet.Extensions;
using CaseManagement.CMMN.CasePlan;
using CaseManagement.CMMN.CasePlan.Exceptions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Apis
{
    [RoutePrefix(CMMNConstants.RouteNames.CasePlans)]
    public class CasePlansController : ApiController
    {
        private readonly ICasePlanService _casePlanService;

        public CasePlansController(ICasePlanService casePlanService)
        {
            _casePlanService = casePlanService;
        }

        [HttpGet]
        [Route("count")]
        public async Task<IHttpActionResult> Count()
        {
            var result = await _casePlanService.Count();
            return Ok(result);
        }


        [HttpGet]
        [Route("me/{id:string}")]
        public async Task<IHttpActionResult> GetMe(string id)
        {
            try
            {
                var result = await _casePlanService.GetMe(id, this.GetNameIdentifier());
                return Ok(result);
            }
            catch (UnknownCasePlanException)
            {
                return NotFound();
            }
            catch (UnauthorizedCasePlanException)
            {
                return Unauthorized();
            }
        }


        [HttpGet]
        [Route("{id:string}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            try
            {
                var result = await _casePlanService.Get(id);
                return Ok(result);
            }
            catch (UnknownCasePlanException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _casePlanService.Search(query);
            return Ok(result);
        }
    }
}
