using CaseManagement.Gateway.Website.AspNetCore.Extensions;
using CaseManagement.Gateway.Website.Statistic.DTOs;
using CaseManagement.Gateway.Website.Statistic.Queries;
using CaseManagement.Gateway.Website.Statistic.QueryHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.AspNetCore.Controllers
{
    [Route(ServerConstants.RouteNames.Statistics)]
    public class StatisticsController : Controller
    {
        private readonly IGetStatisticQueryHandler _statisticQueryRepository;
        private readonly ISearchStatisticQueryHandler _searchStatisticQueryHandler;
        private readonly ICountQueryHandler _countQueryHandler;

        public StatisticsController(IGetStatisticQueryHandler statisticQueryRepository, ISearchStatisticQueryHandler searchStatisticQueryHandler, ICountQueryHandler countQueryHandler)
        {
            _statisticQueryRepository = statisticQueryRepository;
            _searchStatisticQueryHandler = searchStatisticQueryHandler;
            _countQueryHandler = countQueryHandler;
        }

        [HttpGet("count")]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Count()
        {
            var countResult = await _countQueryHandler.Handle(new CountQuery());
            return new OkObjectResult(new JObject
            {
                { "nb_case_files", countResult.NbCaseFiles },
                { "nb_case_plans", countResult.NbCasePlans }
            });
        }

        [HttpGet]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Get()
        {
            var caseDailyStatistic = await _statisticQueryRepository.Handle(new GetStatisticQuery());
            return new OkObjectResult(ToDto(caseDailyStatistic));
        }

        [HttpGet("search")]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var searchDailyStatistics = await _searchStatisticQueryHandler.Handle(new SearchStatisticQuery
            {
                Queries = query
            });
            return new OkObjectResult(ToDto(searchDailyStatistics));
        }
               
        private static JObject ToDto(FindResponse<DailyStatisticResponse> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(DailyStatisticResponse caseStatistic)
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
                { "nb_confirmed_forms", caseStatistic.NbConfirmedForms },
                { "nb_created_forms", caseStatistic.NbCreatedForms }
            };
        }
    }
}