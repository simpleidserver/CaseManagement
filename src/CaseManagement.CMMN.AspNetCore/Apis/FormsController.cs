using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.Form;
using CaseManagement.CMMN.Form.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.Forms)]
    public class FormsController : Controller
    {
        private readonly IFormService _formService;
        
        public FormsController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpGet("{id}")]
        [Authorize("get_form")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _formService.Get(id);
                return new OkObjectResult(result);
            }
            catch(UnknownFormException)
            {
                return new NotFoundResult();
            }
        }

        [HttpGet("search")]
        [Authorize("search_form")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _formService.Search(query);
            return new OkObjectResult(result);
        }

        [HttpGet("{formId}/{version}")]
        [Authorize("get_form")]
        public async Task<IActionResult> Get(string formId, int version)
        {
            try
            {
                var result = await _formService.Get(formId, version);
                return new OkObjectResult(result);
            }
            catch (UnknownFormException)
            {
                return new NotFoundResult();
            }
        }
    }
}