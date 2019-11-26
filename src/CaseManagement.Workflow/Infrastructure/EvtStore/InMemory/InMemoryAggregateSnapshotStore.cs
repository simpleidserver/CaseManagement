using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.EvtStore.InMemory
{
    public class InMemoryAggregateSnapshotStore
    {
        public class AggregateSnapshotStore<T> : IAggregateSnapshotStore<T> where T : BaseAggregate
        {
            private ICollection<SnapshotElement<T>> _snapshots;

            public AggregateSnapshotStore()
            {
                _snapshots = new List<SnapshotElement<T>>();
            }

            public Task<SnapshotElement<T>> GetLast(string id)
            {
                var result = _snapshots.Where(s => s.Content.Id == id).OrderByDescending(s => s.CreateDateTime).FirstOrDefault();
                if (result == null)
                {
                    return Task.FromResult((SnapshotElement<T>)null);
                }

                return Task.FromResult(Copy<T>(result));
            }

            public Task<bool> Add(SnapshotElement<T> snapshot)
            {
                _snapshots.Add(snapshot);
                return Task.FromResult(true);
            }

            public ICollection<SnapshotElement<T>> Query()
            {
                return _snapshots;
            }

            private static SnapshotElement<T> Copy<T>(SnapshotElement<T> obj) where T : BaseAggregate
            {
                var copy = (T)obj.Content.Clone();
                return new SnapshotElement<T>(obj.Start, obj.CreateDateTime, copy);
            }
        }
    }
}
