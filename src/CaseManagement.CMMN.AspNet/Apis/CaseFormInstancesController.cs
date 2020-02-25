using CaseManagement.CMMN.FormInstance;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Apis
{
    [RoutePrefix(CMMNConstants.RouteNames.CaseFormInstances)]
    public class CaseFormInstancesController : ApiController
    {
        private readonly IFormInstanceService _formInstanceService;

        public CaseFormInstancesController(IFormInstanceService formInstanceService)
        {
            _formInstanceService = formInstanceService;
        }


        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _formInstanceService.Search(query);
            return Ok(result);
        }
    }
}