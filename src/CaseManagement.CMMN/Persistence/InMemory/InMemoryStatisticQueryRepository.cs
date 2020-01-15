using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryStatisticQueryRepository : IStatisticQueryRepository
    {
        private static Dictionary<string, string> MAPPING_DAILYSTATISTICNAME_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "datetime", "DateTime" },
            { "nb_activate_cases", "NbActiveCases" },
            { "nb_completed_cases", "NbCompletedCases" },
            { "nb_terminated_cases", "NbTerminatedCases" },
            { "nb_failed_cases", "NbFailedCases" },
            { "nb_suspended_cases", "NbSuspendedCases" },
            { "nb_closed_cases", "NbClosedCases" }
        };
        private static Dictionary<string, string> MAPPING_PERFORMANCESTATISTICNAME_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "datetime", "CaptureDateTime" },
            { "machine_name", "MachineName" },
            { "nb_working_threads", "NbWorkingThreads" }
        };

        private readonly ConcurrentBag<DailyStatisticAggregate> _caseDailyStatistics;
        private readonly ConcurrentBag<PerformanceStatisticAggregate> _performanceStatistics;

        public InMemoryStatisticQueryRepository(ConcurrentBag<DailyStatisticAggregate> caseDailyStatistics, ConcurrentBag<PerformanceStatisticAggregate> performanceStatistics)
        {
            _caseDailyStatistics = caseDailyStatistics;
            _performanceStatistics = performanceStatistics;
        }

        public Task<FindResponse<DailyStatisticAggregate>> FindDailyStatistics(FindDailyStatisticsParameter parameter)
        {
            IQueryable<DailyStatisticAggregate> result = _caseDailyStatistics.AsQueryable();
            if (MAPPING_DAILYSTATISTICNAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_DAILYSTATISTICNAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            if (parameter.StartDateTime != null)
            {
                result = result.Where(fi => parameter.StartDateTime <= fi.DateTime);
            }

            if (parameter.EndDateTime != null)
            {
                result = result.Where(fi => parameter.EndDateTime >= fi.DateTime);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<DailyStatisticAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }

        public Task<IEnumerable<string>> GetMachineNames()
        {
            return Task.FromResult(_performanceStatistics.Select(p => p.MachineName).Distinct());
        }

        public Task<FindResponse<PerformanceStatisticAggregate>> FindPerformanceStatistics(FindPerformanceStatisticsParameter parameter)
        {
            IQueryable<PerformanceStatisticAggregate> result = _performanceStatistics.AsQueryable();
            if (MAPPING_PERFORMANCESTATISTICNAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_PERFORMANCESTATISTICNAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            if (parameter.MachineName != null)
            {
                result = result.Where(fi => parameter.MachineName == fi.MachineName);
            }

            if (parameter.StartDateTime != null)
            {
                result = result.Where(fi => fi.CaptureDateTime >= parameter.StartDateTime);
            }

            List<PerformanceStatisticAggregate> content;
            int totalLength = 0;
            if (parameter.GroupBy == "machine_name")
            {
                var groupedResult = result.GroupBy(p => p.MachineName);
                totalLength = groupedResult.Select(v => v.Count()).Sum();
                content = groupedResult.SelectMany(v => v.Skip(parameter.StartIndex).Take(parameter.Count).ToList()).ToList();
            }
            else
            {
                totalLength = result.Count();
                result = result.Skip(parameter.StartIndex).Take(parameter.Count);
                content = result.ToList();
            }

            return Task.FromResult(new FindResponse<PerformanceStatisticAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content.ToList()
            });
        }
    }
}
