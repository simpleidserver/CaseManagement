using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCMMNActivationCommandRepository : ICMMNActivationCommandRepository
    {
        private List<CaseActivationAggregate> _activations;

        public InMemoryCMMNActivationCommandRepository(List<CaseActivationAggregate> activations)
        {
            _activations = activations;
        }

        public void Add(CaseActivationAggregate activation)
        {
            lock(_activations)
            {
                _activations.Add((CaseActivationAggregate)activation.Clone());
            }
        }

        public void Update(CaseActivationAggregate activation)
        {
            lock(_activations)
            {
                _activations.Remove(_activations.First(a => a.Id == activation.Id));
                _activations.Add((CaseActivationAggregate)activation.Clone());
            }
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
