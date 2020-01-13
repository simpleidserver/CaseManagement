using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryStatisticCommandRepository : IStatisticCommandRepository
    {
        private readonly ConcurrentBag<CaseDefinitionStatisticAggregate> _workflowDefinitionStatistics;
        private readonly ConcurrentBag<DailyStatisticAggregate> _caseDailyStatistics;

        public InMemoryStatisticCommandRepository(ConcurrentBag<CaseDefinitionStatisticAggregate> workflowDefinitionStatistics, ConcurrentBag<DailyStatisticAggregate> caseDailyStatistics)
        {
            _workflowDefinitionStatistics = workflowDefinitionStatistics;
            _caseDailyStatistics = caseDailyStatistics;
        }

        public void Add(CaseDefinitionStatisticAggregate cmmnWorkflowDefinitionStatisticAggregate)
        {
            _workflowDefinitionStatistics.Add((CaseDefinitionStatisticAggregate)cmmnWorkflowDefinitionStatisticAggregate.Clone());
        }

        public void Update(CaseDefinitionStatisticAggregate cmmnWorkflowDefinitionStatisticAggregate)
        {
            var wf = _workflowDefinitionStatistics.First(w => w.CaseDefinitionId == cmmnWorkflowDefinitionStatisticAggregate.CaseDefinitionId);
            _workflowDefinitionStatistics.Remove(wf);
            _workflowDefinitionStatistics.Add((CaseDefinitionStatisticAggregate)wf.Clone());
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
