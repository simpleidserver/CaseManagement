using CaseManagement.Gateway.Website.AspNetCore.Extensions;
using CaseManagement.Gateway.Website.CasePlanInstance.CommandHandlers;
using CaseManagement.Gateway.Website.CasePlanInstance.Commands;
using CaseManagement.Gateway.Website.CasePlanInstance.Queries;
using CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        private readonly IEnableCasePlanElementInstanceCommandHandler _enableCasePlanElementInstanceCommandHandler;
        private readonly IConfirmFormCommandHandler _confirmFormCommandHandler;
        private readonly IEnableAssignedCasePlanElementInstanceCommandHandler _enableAssignedCasePlanElementInstanceCommandHandler;
        private readonly IConfirmAssignedFormCommandHandler _confirmAssignedFormCommandHandler;

        public CasePlanInstancesController(ISearchAssignedCasePlanInstanceQueryHandler searchAssignedCasePlanInstanceQueryHandler, ISearchCasePlanInstanceQueryHandler searchCasePlanInstanceQueryHandler, IGetCasePlanInstanceQueryHandler getCasePlanInstanceQueryHandler, IGetAssignedCasePlanInstanceQueryHandler getAssignedCasePlanInstanceQueryHandler, IEnableCasePlanElementInstanceCommandHandler enableCasePlanElementInstanceCommandHandler, IConfirmFormCommandHandler confirmFormCommandHandler, IEnableAssignedCasePlanElementInstanceCommandHandler enableAssignedCasePlanElementInstanceCommandHandler, IConfirmAssignedFormCommandHandler confirmAssignedFormCommandHandler)
        {
            _searchAssignedCasePlanInstanceQueryHandler = searchAssignedCasePlanInstanceQueryHandler;
            _searchCasePlanInstanceQueryHandler = searchCasePlanInstanceQueryHandler;
            _getCasePlanInstanceQueryHandler = getCasePlanInstanceQueryHandler;
            _getAssignedCasePlanInstanceQueryHandler = getAssignedCasePlanInstanceQueryHandler;
            _enableCasePlanElementInstanceCommandHandler = enableCasePlanElementInstanceCommandHandler;
            _confirmFormCommandHandler = confirmFormCommandHandler;
            _enableAssignedCasePlanElementInstanceCommandHandler = enableAssignedCasePlanElementInstanceCommandHandler;
            _confirmAssignedFormCommandHandler = confirmAssignedFormCommandHandler;
        }

        [HttpGet("search")]
        [Authorize("search_caseplaninstance")]
        public async Task<IActionResult> Search()
        {
            var query = Request.Query.ToEnumerable();
            var result = await _searchCasePlanInstanceQueryHandler.Handle(new SearchCasePlanInstanceQuery { Queries = query });
            return new OkObjectResult(result.ToDto());
        }

        [HttpGet("search/me")]
        [Authorize("me_search_caseplaninstance")]
        public async Task<IActionResult> SearchMe()
        {
            var query = Request.Query.ToEnumerable();
            var result = await _searchAssignedCasePlanInstanceQueryHandler.Handle(new SearchAssignedCasePlanInstanceQuery { IdentityToken = this.GetIdentityToken(), Queries = query });
            return new OkObjectResult(result.ToDto());
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplaninstance")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _getCasePlanInstanceQueryHandler.Handle(new GetCasePlanInstanceQuery { CasePlanInstanceId = id });
            return new OkObjectResult(result.ToDto());
        }

        [HttpGet("me/{id}")]
        [Authorize("me_get_caseplaninstance")]
        public async Task<IActionResult> GetMe(string id)
        {
            var result = await _getAssignedCasePlanInstanceQueryHandler.Handle(new GetAssignedCasePlanInstanceQuery { CasePlanInstanceId = id, IdentityToken = this.GetIdentityToken() });
            return new OkObjectResult(result.ToDto());
        }

        [HttpGet("{id}/enable/{eltId}")]
        [Authorize("enable_caseplaninstance")]
        public async Task<IActionResult> Enable(string id, string eltId)
        {
            await _enableCasePlanElementInstanceCommandHandler.Handle(new EnableCasePlanElementInstanceCommand { CasePlanElementInstanceId = eltId, CasePlanInstanceId = id });
            return new OkResult();
        }

        [HttpGet("me/{id}/enable/{eltId}")]
        [Authorize("me_enable_caseplaninstance")]
        public async Task<IActionResult> EnableMe(string id, string eltId)
        {
            await _enableAssignedCasePlanElementInstanceCommandHandler.Handle(new EnableAssignedCasePlanElementInstanceCommand { CasePlanElementInstanceId = eltId, CasePlanInstanceId = id, IdentityToken = this.GetIdentityToken() });
            return new OkResult();
        }

        [HttpPost("{id}/confirm/{eltId}")]
        [Authorize("confirm_form")]
        public async Task<IActionResult> ConfirmForm(string id, string eltId, [FromBody] JObject content)
        {
            await _confirmFormCommandHandler.Handle(new ConfirmFormCommand { CasePlanInstanceId = id, CasePlanElementInstanceId = eltId, Content = content });
            return new OkResult();
        }

        [HttpPost("me/{id}/confirm/{eltId}")]
        [Authorize("me_confirm_form")]
        public async Task<IActionResult> ConfirmFormMe(string id, string eltId, [FromBody] JObject content)
        {
            await _confirmAssignedFormCommandHandler.Handle(new ConfirmAssignedFormCommand { CasePlanInstanceId = id, CasePlanElementInstanceId = eltId, Content = content, IdentityToken = this.GetIdentityToken() });
            return new OkResult();
        }
    }
}
