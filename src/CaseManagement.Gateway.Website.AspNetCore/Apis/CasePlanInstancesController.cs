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

        public CasePlanInstancesController(ISearchAssignedCasePlanInstanceQueryHandler searchAssignedCasePlanInstanceQueryHandler, ISearchCasePlanInstanceQueryHandler searchCasePlanInstanceQueryHandler)
        {
            _searchAssignedCasePlanInstanceQueryHandler = searchAssignedCasePlanInstanceQueryHandler;
            _searchCasePlanInstanceQueryHandler = searchCasePlanInstanceQueryHandler;
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
    }
}
