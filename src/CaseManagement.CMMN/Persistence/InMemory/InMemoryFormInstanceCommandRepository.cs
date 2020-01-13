using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryFormInstanceCommandRepository : IFormInstanceCommandRepository
    {
        private ConcurrentBag<FormInstanceAggregate> _formInstances;

        public InMemoryFormInstanceCommandRepository(ConcurrentBag<FormInstanceAggregate> formInstances)
        {
            _formInstances = formInstances;
        }

        public void Add(FormInstanceAggregate formInstance)
        {
            _formInstances.Add((FormInstanceAggregate)formInstance.Clone());
        }

        public void Update(FormInstanceAggregate formInstance)
        {
            var record = _formInstances.First(f => f.Id == formInstance.Id);
            _formInstances.Remove(record);
            _formInstances.Add((FormInstanceAggregate)formInstance.Clone());
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
