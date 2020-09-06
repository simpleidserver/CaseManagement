using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Lock
{
    public interface IDistributedLock
    {
        Task<bool> TryAcquireLock(string id, CancellationToken token);
        Task ReleaseLock(string id, CancellationToken token);
    }
}
