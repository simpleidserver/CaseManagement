using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.FormInstance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CaseFormInstances)]
    public class CaseFormInstancesController : Controller
    {
        private readonly IFormInstanceService _formInstanceService;

        public CaseFormInstancesController(IFormInstanceService formInstanceService)
        {
            _formInstanceService = formInstanceService;
        }

        [HttpGet("search")]
        [Authorize("get_forminstances")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _formInstanceService.Search(query);
            return new OkObjectResult(result);
        }
    }
}