using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryDailyStatisticCommandRepository : IDailyStatisticCommandRepository
    {
        private readonly ConcurrentBag<DailyStatisticAggregate> _caseDailyStatistics;

        public InMemoryDailyStatisticCommandRepository(ConcurrentBag<DailyStatisticAggregate> caseDailyStatistics)
        {
            _caseDailyStatistics = caseDailyStatistics;
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

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
