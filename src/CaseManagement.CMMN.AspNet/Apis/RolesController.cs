using CaseManagement.CMMN.AspNet.Extensions;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Roles;
using CaseManagement.CMMN.Roles.Commands;
using CaseManagement.CMMN.Roles.Exceptions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Apis
{
    [RoutePrefix(CMMNConstants.RouteNames.Roles)]
    public class RolesController : ApiController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            try
            {
                var result = await _roleService.Get(id);
                return Ok(result);
            }
            catch (UnknownRoleException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _roleService.Search(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Add([FromBody] AddRoleCommand command)
        {
            try
            {
                var result = await _roleService.Add(command);
                return Content(HttpStatusCode.Created, result);
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            try
            {
                await _roleService.Delete(new DeleteRoleCommand { Role = id });
                return Ok();
            }
            catch (UnknownRoleException)
            {
                return NotFound();
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IHttpActionResult> Update(string id, [FromBody] UpdateRoleCommand updateRoleCommand)
        {
            try
            {
                updateRoleCommand.Role = id;
                await _roleService.Update(updateRoleCommand);
                return Ok();
            }
            catch (UnknownRoleException)
            {
                return NotFound();
            }
            catch (AggregateValidationException ex)
            {
                return Content(HttpStatusCode.BadRequest, this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request));
            }
        }
    }
}
