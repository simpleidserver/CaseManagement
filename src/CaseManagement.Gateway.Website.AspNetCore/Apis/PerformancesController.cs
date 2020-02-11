using CaseManagement.Gateway.Website.AspNetCore.Extensions;
using CaseManagement.Gateway.Website.Performance.DTOs;
using CaseManagement.Gateway.Website.Performance.Queries;
using CaseManagement.Gateway.Website.Performance.QueryHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.AspNetCore.Apis
{
    [Route(ServerConstants.RouteNames.Performances)]
    public class PerformancesController : Controller
    {
        private readonly IGetPerformanceQueryHandler _getPerformanceQueryHandler;
        private readonly ISearchPerformanceQueryHandler _searchPerformanceQueryHandler;

        public PerformancesController(IGetPerformanceQueryHandler getPerformanceQueryHandler, ISearchPerformanceQueryHandler searchPerformanceQueryHandler)
        {
            _getPerformanceQueryHandler = getPerformanceQueryHandler;
            _searchPerformanceQueryHandler = searchPerformanceQueryHandler;
        }

        [HttpGet]
        [Authorize("get_performance")]
        public async Task<IActionResult> Get()
        {
            var performance = await _getPerformanceQueryHandler.Handle(new GetPerformanceQuery());
            return new OkObjectResult(performance);
        }

        [HttpGet("search")]
        [Authorize("get_performance")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var searchPerformance = await _searchPerformanceQueryHandler.Handle(new SearchPerformanceQuery
            {
                Queries = query
            });
            return new OkObjectResult(ToDto(searchPerformance));
        }

        private static JObject ToDto(FindResponse<PerformanceResponse> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(PerformanceResponse performance)
        {
            return new JObject
            {
                { "datetime", performance.DateTime },
                { "machine_name", performance.MachineName },
                { "nb_working_threads", performance.NbWorkingThreads },
                { "memory_consumed_mb", performance.MemoryConsumedMB }
            };
        }
    }
}
