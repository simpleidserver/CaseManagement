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
    public class InMemoryDailyStatisticQueryRepository : IStatisticQueryRepository
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

        private readonly ConcurrentBag<DailyStatisticAggregate> _caseDailyStatistics;

        public InMemoryDailyStatisticQueryRepository(ConcurrentBag<DailyStatisticAggregate> caseDailyStatistics)
        {
            _caseDailyStatistics = caseDailyStatistics;
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
                Content = result.Select(r => (DailyStatisticAggregate)r.Clone()).ToList()
            });
        }
    }
}
