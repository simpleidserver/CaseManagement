using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCasePlanCommandRepository : ICasePlanCommandRepository
    {
        private readonly ConcurrentBag<CasePlanAggregate> _caseDefinitions;

        public InMemoryCasePlanCommandRepository(ConcurrentBag<CasePlanAggregate> caseDefinitions)
        {
            _caseDefinitions = caseDefinitions;
        }

        public Task Update(CasePlanAggregate workflowDefinition, CancellationToken token)
        {
            var wf = _caseDefinitions.First(w => w.AggregateId == workflowDefinition.AggregateId);
            _caseDefinitions.Remove(wf);
            _caseDefinitions.Add((CasePlanAggregate)wf.Clone());
            return Task.CompletedTask;
        }

        public Task Add(CasePlanAggregate workflowDefinition, CancellationToken token)
        {
            _caseDefinitions.Add((CasePlanAggregate)workflowDefinition.Clone());
            return Task.CompletedTask;
        }

        public Task Delete(CasePlanAggregate workflowDefinition, CancellationToken token)
        {
            _caseDefinitions.Remove(_caseDefinitions.First(c => c.AggregateId == workflowDefinition.AggregateId));
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}
