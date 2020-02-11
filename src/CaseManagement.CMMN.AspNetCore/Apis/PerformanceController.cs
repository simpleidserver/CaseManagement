using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.Performances)]
    public class PerformanceController : Controller
    {
        private readonly IPerformanceQueryRepository _performanceQueryRepository;

        public PerformanceController(IPerformanceQueryRepository performanceQueryRepository)
        {
            _performanceQueryRepository = performanceQueryRepository;
        }

        [HttpGet]
        [Authorize("get_performance")]
        public async Task<IActionResult> GetPerformances()
        {
            var result = await _performanceQueryRepository.GetMachineNames();
            return new OkObjectResult(result);
        }

        [HttpGet("search")]
        [Authorize("get_performance")]
        public async Task<IActionResult> SearchPerformances()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _performanceQueryRepository.FindPerformance(ExtractFindPerformanceParameter(query));
            return new OkObjectResult(ToDto(result));
        }

        private static JObject ToDto(FindResponse<PerformanceAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(PerformanceAggregate performanceStatistic)
        {
            return new JObject
            {
                { "datetime", performanceStatistic.CaptureDateTime },
                { "machine_name", performanceStatistic.MachineName },
                { "nb_working_threads", performanceStatistic.NbWorkingThreads },
                { "memory_consumed_mb", performanceStatistic.MemoryConsumedMB }
            };
        }

        private static FindPerformanceParameter ExtractFindPerformanceParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            string machineName;
            DateTime startDateTime;
            string groupBy;
            var parameter = new FindPerformanceParameter();
            parameter.ExtractFindParameter(query);
            if (query.TryGet("machine_name", out machineName))
            {
                parameter.MachineName = machineName;
            }

            if (query.TryGet("start_datetime", out startDateTime))
            {
                parameter.StartDateTime = startDateTime;
            }

            if (query.TryGet("group_by", out groupBy))
            {
                parameter.GroupBy = groupBy;
            }

            return parameter;
        }
    }
}
