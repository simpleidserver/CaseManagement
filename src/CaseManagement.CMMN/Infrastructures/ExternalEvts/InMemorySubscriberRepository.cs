using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.ExternalEvts
{
    public class InMemorySubscriberRepository : ISubscriberRepository
    {
        private readonly ConcurrentBag<Subscription> _subscriptions;

        public InMemorySubscriberRepository()
        {
            _subscriptions = new ConcurrentBag<Subscription>();
        }

        public Task<bool> Add(Subscription subscription, CancellationToken cancellationToken)
        {
            _subscriptions.Add(subscription);
            return Task.FromResult(true);
        }

        public Task<bool> Delete(Subscription subscription, CancellationToken cancellationToken)
        {
            var sub = _subscriptions.First(_ => _.CasePlanElementInstanceId == subscription.CasePlanElementInstanceId
            && _.CasePlanInstanceId == subscription.CasePlanInstanceId
            && _.EventName == subscription.EventName);
            _subscriptions.Remove(sub);
            return Task.FromResult(true);
        }

        public Task<Subscription> Get(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token)
        {
            return Task.FromResult(_subscriptions.FirstOrDefault(_ => _.CasePlanElementInstanceId == casePlanElementInstanceId
               && _.CasePlanInstanceId == casePlanInstanceId
               && _.EventName == evtName));
        }

        public async Task<bool> Update(Subscription subscription, CancellationToken cancellationToken)
        {
            await Delete(subscription, cancellationToken);
            await Add(subscription, cancellationToken);
            return true;
        }
    }
}
