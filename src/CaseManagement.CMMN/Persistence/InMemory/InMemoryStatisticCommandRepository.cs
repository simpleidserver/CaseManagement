using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryStatisticCommandRepository : IStatisticCommandRepository
    {
        private readonly ConcurrentBag<CMMNWorkflowDefinitionStatisticAggregate> _workflowDefinitionStatistics;

        public InMemoryStatisticCommandRepository(ConcurrentBag<CMMNWorkflowDefinitionStatisticAggregate> workflowDefinitionStatistics)
        {
            _workflowDefinitionStatistics = workflowDefinitionStatistics;
        }

        public void Update(CMMNWorkflowDefinitionStatisticAggregate cmmnWorkflowDefinitionStatisticAggregate)
        {
            var wf = _workflowDefinitionStatistics.First(w => w.WorkflowDefinitionId == cmmnWorkflowDefinitionStatisticAggregate.WorkflowDefinitionId);
            _workflowDefinitionStatistics.Remove(wf);
            _workflowDefinitionStatistics.Add((CMMNWorkflowDefinitionStatisticAggregate)wf.Clone());
        }

        public void Add(CMMNWorkflowDefinitionStatisticAggregate cmmnWorkflowDefinitionStatisticAggregate)
        {
            _workflowDefinitionStatistics.Add((CMMNWorkflowDefinitionStatisticAggregate)cmmnWorkflowDefinitionStatisticAggregate.Clone());
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
