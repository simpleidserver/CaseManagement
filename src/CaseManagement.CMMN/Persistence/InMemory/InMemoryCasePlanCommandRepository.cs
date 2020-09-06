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

        public void Update(CasePlanAggregate workflowDefinition)
        {
            var wf = _caseDefinitions.First(w => w.Id == workflowDefinition.Id);
            _caseDefinitions.Remove(wf);
            _caseDefinitions.Add((CasePlanAggregate)wf.Clone());
        }

        public void Add(CasePlanAggregate workflowDefinition)
        {
            _caseDefinitions.Add((CasePlanAggregate)workflowDefinition.Clone());
        }

        public void Delete(CasePlanAggregate workflowDefinition)
        {
            _caseDefinitions.Remove(_caseDefinitions.First(c => c.Id == workflowDefinition.Id));
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}
