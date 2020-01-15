using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryStatisticCommandRepository : IStatisticCommandRepository
    {
        private readonly ConcurrentBag<DailyStatisticAggregate> _caseDailyStatistics;
        private readonly ConcurrentBag<PerformanceStatisticAggregate> _performanceStatistics;

        public InMemoryStatisticCommandRepository(ConcurrentBag<DailyStatisticAggregate> caseDailyStatistics, ConcurrentBag<PerformanceStatisticAggregate> performanceStatistics)
        {
            _caseDailyStatistics = caseDailyStatistics;
            _performanceStatistics = performanceStatistics;
        }

        public void Add(DailyStatisticAggregate caseDailyStatistic)
        {
            _caseDailyStatistics.Add((DailyStatisticAggregate)caseDailyStatistic.Clone());
        }

        public void Update(DailyStatisticAggregate caseDailyStatistic)
        {
            _caseDailyStatistics.Remove(caseDailyStatistic);
            _caseDailyStatistics.Add(caseDailyStatistic);
        }

        public void Add(PerformanceStatisticAggregate performanceStatistic)
        {
            _performanceStatistics.Add(performanceStatistic);
        }

        public Task KeepLastRecords(int nbRecords, string machineName)
        {
            var lst = _performanceStatistics.Where(m => m.MachineName == machineName).OrderByDescending(p => p.CaptureDateTime).Skip(nbRecords).Select(s => new
            {
                CaptureDateTime = s.CaptureDateTime,
                MachineName = s.MachineName
            });
            foreach(var record in lst)
            {
                var existingRecord = _performanceStatistics.First(v => v.MachineName == record.MachineName && v.CaptureDateTime == record.CaptureDateTime);
                _performanceStatistics.Remove(existingRecord);
            }

            return Task.CompletedTask;
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
