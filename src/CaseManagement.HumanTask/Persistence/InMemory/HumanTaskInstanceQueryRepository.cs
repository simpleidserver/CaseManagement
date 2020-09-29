using CaseManagement.HumanTask.Domains;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.InMemory
{
    public class HumanTaskInstanceQueryRepository : IHumanTaskInstanceQueryRepository
    {
        private readonly ConcurrentBag<HumanTaskInstanceAggregate> _humanTaskInstances;

        public HumanTaskInstanceQueryRepository(ConcurrentBag<HumanTaskInstanceAggregate> humanTaskInstances)
        {
            _humanTaskInstances = humanTaskInstances;
        }

        public Task<HumanTaskInstanceAggregate> Get(string id, CancellationToken token)
        {
            return Task.FromResult((HumanTaskInstanceAggregate)_humanTaskInstances.FirstOrDefault(_ => _.AggregateId == id)?.Clone());
        }

        public Task<ICollection<HumanTaskInstanceAggregate>> GetPendingLst(CancellationToken token)
        {
            ICollection<HumanTaskInstanceAggregate> result = _humanTaskInstances.Where(_ => _.ActivationDeferralTime != null && _.ActivationDeferralTime <= DateTime.UtcNow && _.State == HumanTaskInstanceStates.CREATED).Select(_ => (HumanTaskInstanceAggregate)_.Clone()).ToList();
            return Task.FromResult(result);
        }
    }
}
