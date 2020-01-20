using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Lock.InMemory
{
    public class InMemoryDistributedLock : IDistributedLock
    {
        private List<string> _locks = new List<string>();

        public Task<bool> IsLocked(string id)
        {
            return Task.FromResult(_locks.Contains(id));
        }

        public Task<bool> AcquireLock(string id)
        {
            lock (_locks)
            {
                if (_locks.Contains(id))
                {
                    return Task.FromResult(false);
                }

                _locks.Add(id);
                return Task.FromResult(true);
            }
        }

        public Task ReleaseLock(string id)
        {
            lock(_locks)
            {
                _locks.Remove(id);
                return Task.CompletedTask;
            }
        }
    }
}
