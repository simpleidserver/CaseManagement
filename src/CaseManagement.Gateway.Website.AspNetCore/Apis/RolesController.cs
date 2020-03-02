using CaseManagement.Gateway.Website.AspNetCore.Extensions;
using CaseManagement.Gateway.Website.Role.CommandHandlers;
using CaseManagement.Gateway.Website.Role.Commands;
using CaseManagement.Gateway.Website.Role.DTOs;
using CaseManagement.Gateway.Website.Role.Queries;
using CaseManagement.Gateway.Website.Role.QueryHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.AspNetCore.Apis
{
    [Route(ServerConstants.RouteNames.Roles)]
    public class RolesController : Controller
    {
        private readonly IAddRoleCommandHandler _addRoleCommandHandler;
        private readonly IUpdateRoleCommandHandler _updateRoleCommandHandler;
        private readonly IDeleteRoleCommandHandler _deleteRoleCommandHandler;
        private readonly IGetRoleQueryHandler _getRoleQueryHandler;
        private readonly ISearchRoleQueryHandler _searchRoleQueryHandler;

        public RolesController(IAddRoleCommandHandler addRoleCommandHandler, IUpdateRoleCommandHandler updateRoleCommandHandler, IDeleteRoleCommandHandler deleteRoleCommandHandler, IGetRoleQueryHandler getRoleQueryHandler, ISearchRoleQueryHandler searchRoleQueryHandler)
        {
            _addRoleCommandHandler = addRoleCommandHandler;
            _updateRoleCommandHandler = updateRoleCommandHandler;
            _deleteRoleCommandHandler = deleteRoleCommandHandler;
            _getRoleQueryHandler = getRoleQueryHandler;
            _searchRoleQueryHandler = searchRoleQueryHandler;
        }

        [HttpPost]
        [Authorize("add_role")]
        public async Task<IActionResult> Add([FromBody] AddRoleParameter parameter)
        {
            var result = await _addRoleCommandHandler.Handle(new AddRoleCommand { Role = parameter.Role });
            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.Created,
                Content = result.ToDto().ToString(),
                ContentType = "application/json"
            };
        }

        [HttpPut("{id}")]
        [Authorize("update_role")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateRoleParameter parameter)
        {
            await _updateRoleCommandHandler.Handle(new UpdateRoleCommand { Role = id, Users = parameter.Users });
            return new OkResult();
        }

        [HttpDelete("{id}")]
        [Authorize("delete_role")]
        public async Task<IActionResult> Delete(string id)
        {
            await _deleteRoleCommandHandler.Handle(new DeleteRoleCommand { Role = id });
            return new OkResult();
        }

        [HttpGet("{id}")]
        [Authorize("get_role")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _getRoleQueryHandler.Handle(new GetRoleQuery { Role = id });
            return new OkObjectResult(result.ToDto());
        }
        
        [HttpGet("search")]
        [Authorize("search_role")]
        public async Task<IActionResult> Search()
        {
            var query = Request.Query.ToEnumerable();
            var result = await _searchRoleQueryHandler.Handle(new SearchRoleQuery { Queries = query});
            return new OkObjectResult(result.ToDto());
        }
    }
}
