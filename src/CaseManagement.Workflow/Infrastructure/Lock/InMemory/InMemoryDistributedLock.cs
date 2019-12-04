using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Lock.InMemory
{
    public class InMemoryDistributedLock : IDistributedLock
    {
        private static object _obj = new object();

        private class BlockAggregate
        {
            public BlockAggregate(string id)
            {
                Id = id;
                IsBlocked = false;
            }

            public string Id { get; set; }
            public bool IsBlocked { get; set; }  
        }

        private List<BlockAggregate> _aggregates = new List<BlockAggregate>();

        public Task<bool> AcquireLock(string id)
        {
            lock (_obj)
            {
                var aggregate = _aggregates.FirstOrDefault(a => a.Id == id);
                if (aggregate == null)
                {
                    aggregate = new BlockAggregate(id);
                    _aggregates.Add(aggregate);
                }

                if (!aggregate.IsBlocked)
                {
                    aggregate.IsBlocked = true;
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
        }

        public Task ReleaseLock(string id)
        {
            var aggregate = _aggregates.First(a => a.Id == id);
            _aggregates.Remove(aggregate);
            return Task.CompletedTask;
        }
    }
}
