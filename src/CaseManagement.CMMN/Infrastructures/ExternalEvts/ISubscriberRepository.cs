using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.ExternalEvts
{
    public interface ISubscriberRepository
    {
        Task<bool> Add(Subscription subscription, CancellationToken cancellationToken);
        Task<bool> Delete(Subscription subscription, CancellationToken cancellationToken);
        Task<bool> Update(Subscription subscription, CancellationToken cancellationToken);
        Task<Subscription> Get(string casePlanInstanceId, string casePlanElementInstanceId, string evtName, CancellationToken token);
    }
}
