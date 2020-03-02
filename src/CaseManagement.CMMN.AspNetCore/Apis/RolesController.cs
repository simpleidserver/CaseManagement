using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Roles;
using CaseManagement.CMMN.Roles.Commands;
using CaseManagement.CMMN.Roles.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.Roles)]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("{id}")]
        [Authorize("get_role")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _roleService.Get(id);
                return new OkObjectResult(result);
            }
            catch(UnknownRoleException)
            {
                return new NotFoundResult();
            }
        }

        [HttpGet("search")]
        [Authorize("search_role")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _roleService.Search(query);
            return new OkObjectResult(result);
        }

        [HttpPost]
        [Authorize("add_role")]
        public async Task<IActionResult> Add([FromBody] AddRoleCommand command)
        {
            try
            {
                var result = await _roleService.Add(command);
                return new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Content = result.ToString(),
                    ContentType = "application/json"
                };
            }
            catch (AggregateValidationException ex) 
            {                
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("delete_role")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _roleService.Delete(new DeleteRoleCommand { Role = id });
                return new OkResult();
            }
            catch(UnknownRoleException)
            {
                return new NotFoundResult();
            }
            catch(AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }

        [HttpPut("{id}")]
        [Authorize("update_role")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateRoleCommand updateRoleCommand)
        {
            try
            {
                updateRoleCommand.Role = id;
                await _roleService.Update(updateRoleCommand);
                return new OkResult();
            }
            catch (UnknownRoleException)
            {
                return new NotFoundResult();
            }
            catch (AggregateValidationException ex)
            {
                return this.ToError(ex.Errors, HttpStatusCode.BadRequest, Request);
            }
        }
    }
}
