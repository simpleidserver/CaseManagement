using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Lock
{
    public interface IDistributedLock
    {
        Task<bool> AcquireLock(string id);
        Task ReleaseLock(string id);
    }
}
