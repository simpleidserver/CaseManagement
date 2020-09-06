using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCaseInstanceCommandRepository : ICasePlanInstanceCommandRepository
    {
        private ConcurrentBag<CasePlanInstanceAggregate> _instances;

        public InMemoryCaseInstanceCommandRepository(ConcurrentBag<CasePlanInstanceAggregate> instances)
        {
            _instances = instances;
        }

        public void Add(CasePlanInstanceAggregate casePlanInstance)
        {
            _instances.Add(casePlanInstance);
        }

        public void Update(CasePlanInstanceAggregate casePlanInstance)
        {
            var instance = _instances.First(_ => _.Id == casePlanInstance.Id);
            _instances.Remove(instance);
            _instances.Add(casePlanInstance);
        }

        public Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            return Task.FromResult(1);
        }
    }
}
