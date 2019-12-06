using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Lock.InMemory
{
    public class InMemoryDistributedLock : IDistributedLock
    {
        private List<string> _locks = new List<string>();

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
