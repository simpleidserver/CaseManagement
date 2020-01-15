using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCaseDefinitionCommandRepository : ICaseDefinitionCommandRepository
    {
        private readonly ConcurrentBag<CaseDefinition> _caseDefinitions;
        private readonly ConcurrentBag<CaseDefinitionHistoryAggregate> _caseDefinitionHistories;

        public InMemoryCaseDefinitionCommandRepository(ConcurrentBag<CaseDefinition> caseDefinitions, ConcurrentBag<CaseDefinitionHistoryAggregate> caseDefinitionHistories)
        {
            _caseDefinitions = caseDefinitions;
            _caseDefinitionHistories = caseDefinitionHistories;
        }

        public void Update(CaseDefinition workflowDefinition)
        {
            throw new System.NotImplementedException();
        }

        public void Add(CaseDefinitionHistoryAggregate caseDefinitionHistory)
        {
            _caseDefinitionHistories.Add((CaseDefinitionHistoryAggregate)caseDefinitionHistory.Clone());
        }

        public void Update(CaseDefinitionHistoryAggregate caseDefinitionHistory)
        {
            var wf = _caseDefinitionHistories.First(w => w.CaseDefinitionId == caseDefinitionHistory.CaseDefinitionId);
            _caseDefinitionHistories.Remove(wf);
            _caseDefinitionHistories.Add((CaseDefinitionHistoryAggregate)wf.Clone());
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
