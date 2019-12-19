using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryFormInstanceCommandRepository : IFormInstanceCommandRepository
    {
        private List<FormInstanceAggregate> _formInstances;

        public InMemoryFormInstanceCommandRepository(List<FormInstanceAggregate> formInstances)
        {
            _formInstances = formInstances;
        }

        public void Add(FormInstanceAggregate formInstance)
        {
            lock (_formInstances)
            {
                _formInstances.Add((FormInstanceAggregate)formInstance.Clone());
            }
        }

        public void Update(FormInstanceAggregate formInstance)
        {
            lock (_formInstances)
            {
                var record = _formInstances.First(f => f.Id == formInstance.Id);
                _formInstances.Remove(record);
                _formInstances.Add((FormInstanceAggregate)formInstance.Clone());
            }
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
