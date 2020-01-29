using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Controllers
{
    [Route(CMMNConstants.RouteNames.Statistics)]
    public class StatisticsController : Controller
    {
        private readonly IStatisticQueryRepository _statisticQueryRepository;

        public StatisticsController(IStatisticQueryRepository statisticQueryRepository)
        {
            _statisticQueryRepository = statisticQueryRepository;
        }

        [HttpGet]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Get()
        {
            var currentDateTime = DateTime.UtcNow.Date;
            var result = await _statisticQueryRepository.FindDailyStatistics(new FindDailyStatisticsParameter
            {
                StartDateTime = currentDateTime,
                EndDateTime = currentDateTime
            });
            DailyStatisticAggregate caseDailyStatistic = null;
            if (result.TotalLength == 0)
            {
                caseDailyStatistic = new DailyStatisticAggregate
                {
                    DateTime = currentDateTime
                };
            }
            else
            {
                caseDailyStatistic = result.Content.First();
            }

            return new OkObjectResult(ToDto(caseDailyStatistic));
        }

        [HttpGet("search")]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _statisticQueryRepository.FindDailyStatistics(ExtractFindParameter(query));
            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("performances")]
        [Authorize("get_performance")]
        public async Task<IActionResult> GetPerformances()
        {
            var result = await _statisticQueryRepository.GetMachineNames();
            return new OkObjectResult(result);
        }

        [HttpGet("performances/search")]
        [Authorize("get_performance")]
        public async Task<IActionResult> SearchPerformances()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _statisticQueryRepository.FindPerformanceStatistics(ExtractFindPerformanceParameter(query));
            return new OkObjectResult(ToDto(result));
        }

        private static JObject ToDto(FindResponse<DailyStatisticAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(FindResponse<PerformanceStatisticAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(DailyStatisticAggregate caseStatistic)
        {
            return new JObject
            {
                { "datetime", caseStatistic.DateTime },
                { "nb_active_cases", caseStatistic.NbActiveCases },
                { "nb_completed_cases", caseStatistic.NbCompletedCases },
                { "nb_terminated_cases", caseStatistic.NbTerminatedCases },
                { "nb_failed_cases", caseStatistic.NbFailedCases },
                { "nb_suspended_cases", caseStatistic.NbSuspendedCases },
                { "nb_closed_cases", caseStatistic.NbClosedCases },
                { "nb_confirmed_forms", caseStatistic.NbConfirmedForm },
                { "nb_created_forms", caseStatistic.NbCreatedForm },
                { "nb_created_activations", caseStatistic.NbCreatedActivation },
                { "nb_confirmed_activations", caseStatistic.NbConfirmedActivation }
            };
        }

        private static JObject ToDto(PerformanceStatisticAggregate performanceStatistic)
        {
            return new JObject
            {
                { "datetime", performanceStatistic.CaptureDateTime },
                { "machine_name", performanceStatistic.MachineName },
                { "nb_working_threads", performanceStatistic.NbWorkingThreads },
                { "memory_consumed_mb", performanceStatistic.MemoryConsumedMB }
            };
        }

        private static FindDailyStatisticsParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            DateTime startDateTime;
            DateTime endDateTime;
            var parameter = new FindDailyStatisticsParameter();
            parameter.ExtractFindParameter(query);
            if (query.TryGet("start_datetime", out startDateTime))
            {
                parameter.StartDateTime = startDateTime;
            }

            if (query.TryGet("end_datetime", out endDateTime))
            {
                parameter.EndDateTime = endDateTime;
            }

            return parameter;
        }

        private static FindPerformanceStatisticsParameter ExtractFindPerformanceParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            string machineName;
            DateTime startDateTime;
            string groupBy;
            var parameter = new FindPerformanceStatisticsParameter();
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