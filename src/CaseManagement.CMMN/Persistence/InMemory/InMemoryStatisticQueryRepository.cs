using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryStatisticQueryRepository : IStatisticQueryRepository
    {
        private readonly ConcurrentBag<CMMNWorkflowDefinitionStatisticAggregate> _workflowDefinitionStatistics;

        public InMemoryStatisticQueryRepository(ConcurrentBag<CMMNWorkflowDefinitionStatisticAggregate> workflowDefinitionStatistics)
        {
            _workflowDefinitionStatistics = workflowDefinitionStatistics;
        }

        public Task<CMMNWorkflowDefinitionStatisticAggregate> FindById(string id)
        {
            return Task.FromResult(_workflowDefinitionStatistics.FirstOrDefault(w => w.WorkflowDefinitionId == id));
        }
    }
}
