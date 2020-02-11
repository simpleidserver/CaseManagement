using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
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

        public void Add(Domains.CasePlanInstanceAggregate workflowInstance)
        {
            _instances.Add((Domains.CasePlanInstanceAggregate)workflowInstance.Clone());
        }

        public void Update(Domains.CasePlanInstanceAggregate workflowInstance)
        {
            var instance = _instances.First(i => i.Id == workflowInstance.Id);
            _instances.Remove(instance);
            _instances.Add((Domains.CasePlanInstanceAggregate)workflowInstance.Clone());
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
