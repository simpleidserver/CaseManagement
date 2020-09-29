using CaseManagement.HumanTask.Domains;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.InMemory
{
    public class HumanTaskInstanceCommandRepository : IHumanTaskInstanceCommandRepository
    {
        private readonly ConcurrentBag<HumanTaskInstanceAggregate> _humanTaskInstances;

        public HumanTaskInstanceCommandRepository(ConcurrentBag<HumanTaskInstanceAggregate> humanTaskInstances)
        {
            _humanTaskInstances = humanTaskInstances;
        }

        public Task<bool> Add(HumanTaskInstanceAggregate humanTaskInstance, CancellationToken token)
        {
            _humanTaskInstances.Add((HumanTaskInstanceAggregate)humanTaskInstance.Clone());
            return Task.FromResult(true);
        }

        public Task<bool> Update(HumanTaskInstanceAggregate humanTaskInstance, CancellationToken token)
        {
            var record = _humanTaskInstances.FirstOrDefault(_ => _.AggregateId == humanTaskInstance.AggregateId);
            if (record == null)
            {
                return Task.FromResult(false);
            }

            _humanTaskInstances.Remove(record);
            return Task.FromResult(true);
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}
