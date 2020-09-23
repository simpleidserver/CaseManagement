using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.InMemory
{
    public class InMemoryProcessInstanceCommandRepository : IProcessInstanceCommandRepository
    {
        private ConcurrentBag<Domains.ProcessInstanceAggregate> _instances;

        public InMemoryProcessInstanceCommandRepository(ConcurrentBag<Domains.ProcessInstanceAggregate> instances)
        {
            _instances = instances;
        }

        public Task Add(Domains.ProcessInstanceAggregate workflowInstance, CancellationToken token)
        {
            _instances.Add((Domains.ProcessInstanceAggregate)workflowInstance.Clone());
            return Task.CompletedTask;
        }

        public Task Update(Domains.ProcessInstanceAggregate workflowInstance, CancellationToken token)
        {
            var instance = _instances.First(i => i.AggregateId == workflowInstance.AggregateId);
            var clone = (Domains.ProcessInstanceAggregate)workflowInstance.Clone();
            _instances.Remove(instance);
            _instances.Add(clone);
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}
