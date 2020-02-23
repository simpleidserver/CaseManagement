using CaseManagement.Gateway.Website.AspNetCore.Extensions;
using CaseManagement.Gateway.Website.CasePlanInstance.Queries;
using CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.AspNetCore.Apis
{
    [Route(ServerConstants.RouteNames.CasePlanInstances)]
    public class CasePlanInstancesController : Controller
    {
        private readonly ISearchAssignedCasePlanInstanceQueryHandler _searchAssignedCasePlanInstanceQueryHandler;
        private readonly ISearchCasePlanInstanceQueryHandler _searchCasePlanInstanceQueryHandler;
        private readonly IGetCasePlanInstanceQueryHandler _getCasePlanInstanceQueryHandler;
        private readonly IGetAssignedCasePlanInstanceQueryHandler _getAssignedCasePlanInstanceQueryHandler;

        public CasePlanInstancesController(ISearchAssignedCasePlanInstanceQueryHandler searchAssignedCasePlanInstanceQueryHandler, ISearchCasePlanInstanceQueryHandler searchCasePlanInstanceQueryHandler, IGetCasePlanInstanceQueryHandler getCasePlanInstanceQueryHandler, IGetAssignedCasePlanInstanceQueryHandler getAssignedCasePlanInstanceQueryHandler)
        {
            _searchAssignedCasePlanInstanceQueryHandler = searchAssignedCasePlanInstanceQueryHandler;
            _searchCasePlanInstanceQueryHandler = searchCasePlanInstanceQueryHandler;
            _getCasePlanInstanceQueryHandler = getCasePlanInstanceQueryHandler;
            _getAssignedCasePlanInstanceQueryHandler = getAssignedCasePlanInstanceQueryHandler;
        }

        [HttpGet("search")]
        [Authorize("search_caseplaninstance")]
        public async Task<IActionResult> Search()
        {
            var query = Request.Query.ToEnumerable();
            var result = await _searchCasePlanInstanceQueryHandler.Handle(new SearchCasePlanInstanceQuery { Queries = query });
            return new OkObjectResult(CasePlansController.ToDto(result));
        }

        [HttpGet("search/me")]
        [Authorize("me_search_caseplaninstance")]
        public async Task<IActionResult> SearchMe()
        {
            var query = Request.Query.ToEnumerable();
            var result = await _searchAssignedCasePlanInstanceQueryHandler.Handle(new SearchAssignedCasePlanInstanceQuery { IdentityToken = this.GetIdentityToken(), Queries = query });
            return new OkObjectResult(CasePlansController.ToDto(result));
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplaninstance")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _getCasePlanInstanceQueryHandler.Handle(new GetCasePlanInstanceQuery { CasePlanInstanceId = id });
            return new OkObjectResult(CasePlansController.ToDto(result));
        }

        [HttpGet("me/{id}")]
        [Authorize("me_get_caseplaninstance")]
        public async Task<IActionResult> GetMe(string id)
        {
            var result = await _getAssignedCasePlanInstanceQueryHandler.Handle(new GetAssignedCasePlanInstanceQuery { CasePlanInstanceId = id, IdentityToken = this.GetIdentityToken() });
            return new OkObjectResult(CasePlansController.ToDto(result));
        }
    }
}
