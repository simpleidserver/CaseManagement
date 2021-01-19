using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.ExternalEvts
{
    public class InMemorySubscriberRepository : ISubscriberRepository
    {
        private readonly ConcurrentBag<Subscription> _subscriptions;

        public InMemorySubscriberRepository()
        {
            _subscriptions = new ConcurrentBag<Subscription>();
        }

        public Task<Subscription> Get(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token)
        {
            return Task.FromResult(_subscriptions.FirstOrDefault(_ => _.CasePlanElementInstanceId == casePlanElementInstanceId
               && _.CasePlanInstanceId == casePlanInstanceId
               && _.EventName == evtName));
        }

        public async Task<bool> Update(Subscription subscription, CancellationToken cancellationToken)
        {
            var record = await Get(subscription.CasePlanInstanceId, subscription.CasePlanElementInstanceId, subscription.EventName, cancellationToken);
            _subscriptions.Remove(record);
            _subscriptions.Add(subscription);
            return true;
        }

        public Task<Subscription> TrySubscribe(string casePlanInstanceId, string evtName, CancellationToken token)
        {
            var result = _subscriptions.FirstOrDefault(_ => _.EventName == evtName && _.CasePlanInstanceId == casePlanInstanceId && _.CasePlanElementInstanceId == null);
            return CommonTrySubscribe(casePlanInstanceId, null, evtName, result, token);
        }

        public Task<Subscription> TrySubscribe(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token)
        {
            var result = _subscriptions.FirstOrDefault(_ => _.EventName == evtName && _.CasePlanInstanceId == casePlanInstanceId && _.CasePlanElementInstanceId == casePlanElementInstanceId);
            return CommonTrySubscribe(casePlanInstanceId, casePlanElementInstanceId, evtName, result, token);
        }

        public Task<Subscription> TryReset(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token)
        {
            var result = _subscriptions.FirstOrDefault(_ => _.EventName == evtName && _.CasePlanInstanceId == casePlanInstanceId && _.CasePlanElementInstanceId == casePlanElementInstanceId);
            if (result == null)
            {
                return Task.FromResult((Subscription)null);
            }

            result.IsCaptured = false;
            return Task.FromResult(result);
        }


        private Task<Subscription> CommonTrySubscribe(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, Subscription result, CancellationToken token)
        {
            if (result == null)
            {
                result = new Subscription
                {
                    CreationDateTime = DateTime.UtcNow,
                    CasePlanElementInstanceId = casePlanElementInstanceId,
                    CasePlanInstanceId = casePlanInstanceId,
                    EventName = evtName
                };
                _subscriptions.Add(result);
            }

            return Task.FromResult(result);
        }
    }
}
