using CaseManagement.Gateway.Website.AspNetCore.Extensions;
using CaseManagement.Gateway.Website.CasePlans.DTOs;
using CaseManagement.Gateway.Website.CasePlans.Queries;
using CaseManagement.Gateway.Website.CasePlans.QueryHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.AspNetCore.Apis
{
    [Route(ServerConstants.RouteNames.CasePlans)]
    public class CasePlansController : Controller
    {
        private readonly ISearchMyLatestCasePlanQueryHandler _searchMyLatestCasePlanQueryHandler;
        private readonly IGetCasePlanQueryHandler _getCasePlanQueryHandler;

        public CasePlansController(ISearchMyLatestCasePlanQueryHandler searchMyLatestCasePlanQueryHandler, IGetCasePlanQueryHandler getCasePlanQueryHandler)
        {
            _searchMyLatestCasePlanQueryHandler = searchMyLatestCasePlanQueryHandler;
            _getCasePlanQueryHandler = getCasePlanQueryHandler;
        }

        [HttpGet("me/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchMyLatest()
        {
            var query = Request.Query.ToEnumerable();
            var nameIdentifier = this.GetNameIdentifier();
            var result = await _searchMyLatestCasePlanQueryHandler.Handle(new SearchMyLatestCasePlanQuery { Queries = query, NameIdentifier = nameIdentifier });
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _getCasePlanQueryHandler.Handle(id, this.GetIdentityToken());
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/caseplaninstances/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchCasePlanInstances(string id)
        {
            return null;
        }

        [HttpGet("{id}/forminstances/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchCaseFormInstances(string id)
        {
            return null;
        }

        [HttpGet("{id}/caseworkertasks/search")]
        [Authorize("get_caseplan")]
        public async Task<IActionResult> SearchCaseWorkerTasks(string id)
        {
            return null;
        }

        private static JObject ToDto(FindResponse<CasePlanResponse> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(CasePlanResponse def)
        {
            return new JObject
            {
                { "id", def.Id },
                { "name", def.Name },
                { "description", def.Description },
                { "case_file", def.CaseFileId },
                { "create_datetime", def.CreateDateTime },
                { "version", def.Version },
                { "owner", def.Owner }
            };
        }
    }
}
