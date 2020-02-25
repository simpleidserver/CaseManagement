using CaseManagement.CMMN.Form;
using CaseManagement.CMMN.Form.Exceptions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Apis
{
    [RoutePrefix(CMMNConstants.RouteNames.Forms)]
    public class FormsController : ApiController
    {
        private readonly IFormService _formService;

        public FormsController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpGet]
        [Route("{id:string}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            try
            {
                var result = await _formService.Get(id);
                return Ok(result);
            }
            catch (UnknownFormException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _formService.Search(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("{formId:string}/{version:int}")]
        public async Task<IHttpActionResult> Get(string formId, int version)
        {
            try
            {
                var result = await _formService.Get(formId, version);
                return Ok(result);
            }
            catch (UnknownFormException)
            {
                return NotFound();
            }
        }
    }
}
