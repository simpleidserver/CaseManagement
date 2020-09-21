using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Lock
{
    public interface IDistributedLock
    {
        Task<bool> TryAcquireLock(string id, CancellationToken token);
        Task ReleaseLock(string id, CancellationToken token);
    }
}
