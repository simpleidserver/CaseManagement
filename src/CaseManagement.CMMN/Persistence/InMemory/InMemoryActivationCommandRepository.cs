using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryActivationCommandRepository : IActivationCommandRepository
    {
        private ConcurrentBag<CaseActivationAggregate> _activations;

        public InMemoryActivationCommandRepository(ConcurrentBag<CaseActivationAggregate> activations)
        {
            _activations = activations;
        }

        public void Delete(CaseActivationAggregate activation)
        {
            _activations.Remove(_activations.First(a => a.WorkflowElementInstanceId == activation.WorkflowElementInstanceId));
        }

        public void Add(CaseActivationAggregate activation)
        {
            _activations.Add((CaseActivationAggregate)activation.Clone());
        }

        public void Update(CaseActivationAggregate activation)
        {
            _activations.Remove(_activations.First(a => a.WorkflowElementInstanceId == activation.WorkflowElementInstanceId));
            _activations.Add((CaseActivationAggregate)activation.Clone());
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
