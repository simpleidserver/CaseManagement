using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Lock
{
    public interface IDistributedLock
    {
        Task<bool> IsLocked(string id);
        Task<bool> AcquireLock(string id);
        Task ReleaseLock(string id);
    }
}
