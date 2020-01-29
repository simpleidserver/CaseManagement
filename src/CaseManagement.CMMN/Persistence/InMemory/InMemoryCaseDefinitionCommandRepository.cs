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
            var wf = _caseDefinitions.First(w => w.Id == workflowDefinition.Id);
            _caseDefinitions.Remove(wf);
            _caseDefinitions.Add((CaseDefinition)wf.Clone());
        }

        public void Add(CaseDefinition workflowDefinition)
        {
            _caseDefinitions.Add((CaseDefinition)workflowDefinition.Clone());
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

        public void Delete(CaseDefinition workflowDefinition)
        {
            _caseDefinitions.Remove(_caseDefinitions.First(c => c.Id == workflowDefinition.Id));
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
