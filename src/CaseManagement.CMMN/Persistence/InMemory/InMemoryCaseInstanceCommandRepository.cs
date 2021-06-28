using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCaseInstanceCommandRepository : ICasePlanInstanceCommandRepository
    {
        private ConcurrentBag<Domains.CasePlanInstanceAggregate> _instances;

        public InMemoryCaseInstanceCommandRepository(ConcurrentBag<Domains.CasePlanInstanceAggregate> instances)
        {
            _instances = instances;
        }

        public Task<Domains.CasePlanInstanceAggregate> Get(string id, CancellationToken token)
        {
            return Task.FromResult(_instances.FirstOrDefault(_ => _.AggregateId == id));
        }

        public Task Add(Domains.CasePlanInstanceAggregate workflowInstance, CancellationToken token)
        {
            _instances.Add((Domains.CasePlanInstanceAggregate)workflowInstance.Clone());
            return Task.CompletedTask;
        }

        public Task Update(Domains.CasePlanInstanceAggregate workflowInstance, CancellationToken token)
        {
            var instance = _instances.First(i => i.AggregateId == workflowInstance.AggregateId);
            _instances.Remove(instance);
            _instances.Add((Domains.CasePlanInstanceAggregate)workflowInstance.Clone());
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}
