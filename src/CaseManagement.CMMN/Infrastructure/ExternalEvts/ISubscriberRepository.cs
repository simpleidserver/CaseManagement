using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.ExternalEvts
{
    public interface ISubscriberRepository
    {
        Task<Subscription> Get(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token);
        Task<bool> Update(Subscription subscription, CancellationToken cancellationToken);
        Task<Subscription> TrySubscribe(string casePlanInstanceId, string evtName, CancellationToken token);
        Task<Subscription> TrySubscribe(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token);
        Task<Subscription> TryReset(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token);
    }
}
