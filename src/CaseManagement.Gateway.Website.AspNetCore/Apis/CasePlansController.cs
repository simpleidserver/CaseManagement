using CaseManagement.Gateway.Website.AspNetCore.Extensions;
using CaseManagement.Gateway.Website.CasePlans.CommandHandlers;
using CaseManagement.Gateway.Website.CasePlans.Commands;
using CaseManagement.Gateway.Website.CasePlans.Queries;
using CaseManagement.Gateway.Website.CasePlans.QueryHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.AspNetCore.Apis
{
    [Route(ServerConstants.RouteNames.CasePlans)]
    public class CasePlansController : Controller
    {
        private readonly ISearchMyLatestCasePlanQueryHandler _searchMyLatestCasePlanQueryHandler;
        private readonly IGetCasePlanQueryHandler _getCasePlanQueryHandler;
        private readonly ILaunchCasePlanInstanceCommandHandler _launchCasePlanInstanceCommandHandler;
        private readonly ISearchCasePlanInstanceQueryHandler _searchCasePlanInstanceQueryHandler;
        private readonly ISearchFormInstanceQueryHandler _searchFormInstanceQueryHandler;
        private readonly ISearchCaseWorkerTaskQueryHandler _searchCaseWorkerTaskQueryHandler;
        private readonly ICloseCasePlanInstanceCommandHandler _closeCasePlanInstanceCommandHandler;
        private readonly IReactivateCasePlanInstanceCommandHandler _reactivateCasePlanInstanceCommandHandler;
        private readonly IResumeCasePlanInstanceCommandHandler _resumeCasePlanInstanceCommandHandler;
        private readonly ISuspendCasePlanInstanceCommandHandler _suspendCasePlanInstanceCommandHandler;
        private readonly ITerminateCasePlanInstanceCommandHandler _terminateCasePlanInstanceCommandHandler;
        private readonly ISearchCasePlanHistoryQueryHandler _searchCasePlanHistoryQueryHandler;

        public CasePlansController(ISearchMyLatestCasePlanQueryHandler searchMyLatestCasePlanQueryHandler, IGetCasePlanQueryHandler getCasePlanQueryHandler, ILaunchCasePlanInstanceCommandHandler launchCasePlanInstanceCommandHandler, ISearchCasePlanInstanceQueryHandler searchCasePlanInstanceQueryHandler, ISearchFormInstanceQueryHandler searchFormInstanceQueryHandler, ISearchCaseWorkerTaskQueryHandler searchCaseWorkerTaskQueryHandler, ICloseCasePlanInstanceCommandHandler closeCasePlanInstanceCommandHandler, IReactivateCasePlanInstanceCommandHandler reactivateCasePlanInstanceCommandHandler, IResumeCasePlanInstanceCommandHandler resumeCasePlanInstanceCommandHandler, ISuspendCasePlanInstanceCommandHandler suspendCasePlanInstanceCommandHandler, ITerminateCasePlanInstanceCommandHandler terminateCasePlanInstanceCommandHandler, ISearchCasePlanHistoryQueryHandler searchCasePlanHistoryQueryHandler)
        {
            _searchMyLatestCasePlanQueryHandler = searchMyLatestCasePlanQueryHandler;
            _getCasePlanQueryHandler = getCasePlanQueryHandler;
            _launchCasePlanInstanceCommandHandler = launchCasePlanInstanceCommandHandler;
            _searchCasePlanInstanceQueryHandler = searchCasePlanInstanceQueryHandler;
            _searchFormInstanceQueryHandler = searchFormInstanceQueryHandler;
            _searchCaseWorkerTaskQueryHandler = searchCaseWorkerTaskQueryHandler;
            _closeCasePlanInstanceCommandHandler = closeCasePlanInstanceCommandHandler;
            _reactivateCasePlanInstanceCommandHandler = reactivateCasePlanInstanceCommandHandler;
            _resumeCasePlanInstanceCommandHandler = resumeCasePlanInstanceCommandHandler;
            _suspendCasePlanInstanceCommandHandler = suspendCasePlanInstanceCommandHandler;
            _terminateCasePlanInstanceCommandHandler = terminateCasePlanInstanceCommandHandler;
            _searchCasePlanHistoryQueryHandler = searchCasePlanHistoryQueryHandler;
        }

        [HttpGet("me/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchMyLatest()
        {
            var query = Request.Query.ToEnumerable();
            var nameIdentifier = this.GetNameIdentifier();
            var result = await _searchMyLatestCasePlanQueryHandler.Handle(new SearchMyLatestCasePlanQuery { Queries = query, NameIdentifier = nameIdentifier });
            return new OkObjectResult(result.ToDto());
        }

        [HttpGet("{id}/history/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchHistory()
        {
            var query = Request.Query.ToEnumerable();
            var nameIdentifier = this.GetNameIdentifier();
            var result = await _searchCasePlanHistoryQueryHandler.Handle(new SearchCasePlanHistoryQuery { Queries = query, NameIdentifier = nameIdentifier });
            return new OkObjectResult(result.ToDto());
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _getCasePlanQueryHandler.Handle(id, this.GetIdentityToken());
            return new OkObjectResult(result.ToDto());
        }

        [HttpGet("{id}/caseplaninstances/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchCasePlanInstances(string id)
        {
            var query = Request.Query.ToEnumerable();
            var nameIdentifier = this.GetNameIdentifier();
            var result = await _searchCasePlanInstanceQueryHandler.Handle(new SearchCasePlanInstanceQuery { CasePlanId = id, Queries = query, Owner = nameIdentifier });
            return new OkObjectResult(result.ToDto());
        }

        [HttpGet("{id}/forminstances/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchCaseFormInstances(string id)
        {
            var query = Request.Query.ToEnumerable();
            var result = await _searchFormInstanceQueryHandler.Handle(new SearchFormInstanceQuery { CasePlanId = id, Queries = query });
            return new OkObjectResult(result.ToDto());
        }

        [HttpGet("{id}/caseworkertasks/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchCaseWorkerTasks(string id)
        {
            var query = Request.Query.ToEnumerable();
            var result = await _searchCaseWorkerTaskQueryHandler.Handle(new SearchCaseWorkerTaskQuery { CasePlanId = id, Queries = query });
            return new OkObjectResult(result.ToDto());
        }

        [HttpGet("{id}/caseplaninstances/launch")]
        [Authorize("launch_caseplaninstance")]
        public async Task<IActionResult> Launch(string id)
        {
            var result = await _launchCasePlanInstanceCommandHandler.Handle(id, this.GetIdentityToken());
            return new OkObjectResult(result.ToDto());
        }

        [HttpGet("{id}/caseplaninstances/{casePlanInstanceId}/close")]
        [Authorize("close_caseplaninstance")]
        public async Task<IActionResult> Close(string id, string casePlanInstanceId)
        {
            await _closeCasePlanInstanceCommandHandler.Handle(new CloseCasePlanInstanceCommand(casePlanInstanceId, this.GetIdentityToken()));
            return new OkResult();
        }

        [HttpGet("{id}/caseplaninstances/{casePlanInstanceId}/reactivate")]
        [Authorize("reactivate_caseplaninstance")]
        public async Task<IActionResult> Reactivate(string id, string casePlanInstanceId)
        {
            await _reactivateCasePlanInstanceCommandHandler.Handle(new ReactivateCasePlanInstanceCommand(casePlanInstanceId, this.GetIdentityToken()));
            return new OkResult();
        }

        [HttpGet("{id}/caseplaninstances/{casePlanInstanceId}/resume")]
        [Authorize("resume_caseplaninstance")]
        public async Task<IActionResult> Resume(string id, string casePlanInstanceId)
        {
            await _resumeCasePlanInstanceCommandHandler.Handle(new ResumeCasePlanInstanceCommand(casePlanInstanceId, this.GetIdentityToken()));
            return new OkResult();
        }

        [HttpGet("{id}/caseplaninstances/{casePlanInstanceId}/suspend")]
        [Authorize("suspend_caseplaninstance")]
        public async Task<IActionResult> Suspend(string id, string casePlanInstanceId)
        {
            await _suspendCasePlanInstanceCommandHandler.Handle(new SuspendCasePlanInstanceCommand(casePlanInstanceId, this.GetIdentityToken()));
            return new OkResult();
        }

        [HttpGet("{id}/caseplaninstances/{casePlanInstanceId}/terminate")]
        [Authorize("terminate_caseplaninstance")]
        public async Task<IActionResult> Terminate(string id, string casePlanInstanceId)
        {
            await _terminateCasePlanInstanceCommandHandler.Handle(new TerminateCasePlanInstanceCommand(casePlanInstanceId, this.GetIdentityToken()));
            return new OkResult();
        }
    }
}
