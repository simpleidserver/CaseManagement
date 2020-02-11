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
    public class InMemoryPerformanceQueryRepository : IPerformanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_PERFORMANCESTATISTICNAME_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "datetime", "CaptureDateTime" },
            { "machine_name", "MachineName" },
            { "nb_working_threads", "NbWorkingThreads" }
        };

        private readonly ConcurrentBag<PerformanceAggregate> _performanceStatistics;

        public InMemoryPerformanceQueryRepository(ConcurrentBag<PerformanceAggregate> performanceStatistics)
        {
            _performanceStatistics = performanceStatistics;
        }

        public Task<IEnumerable<string>> GetMachineNames()
        {
            return Task.FromResult(_performanceStatistics.Select(p => p.MachineName).Distinct());
        }

        public Task<FindResponse<PerformanceAggregate>> FindPerformance(FindPerformanceParameter parameter)
        {
            IQueryable<PerformanceAggregate> result = _performanceStatistics.AsQueryable();
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

            List<PerformanceAggregate> content;
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

            return Task.FromResult(new FindResponse<PerformanceAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content.Select(c => (PerformanceAggregate)c.Clone()).ToList()
            });
        }
    }
}
