using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.EvtStore
{
    public class InMemoryAggregateSnapshotStore : IAggregateSnapshotStore
    {
        private ConcurrentBag<SnapshotElement<BaseAggregate>> _snapshots;

        public InMemoryAggregateSnapshotStore()
        {
            _snapshots = new ConcurrentBag<SnapshotElement<BaseAggregate>>();
        }

        public Task<SnapshotElement<T>> GetLast<T>(string id, CancellationToken cancellationToken) where T : BaseAggregate
        {
            var result = _snapshots.Where(s => s.Id == id).OrderByDescending(s => s.CreateDateTime).FirstOrDefault();
            if (result == null)
            {
                return Task.FromResult((SnapshotElement<T>)null);
            }

            return Task.FromResult(Copy<T>(result));
        }

        public Task<bool> Add(SnapshotElement<BaseAggregate> snapshot, CancellationToken cancellationToken)
        {
            _snapshots.Add(snapshot);
            return Task.FromResult(true);
        }

        public ICollection<SnapshotElement<T>> Query<T>() where T : BaseAggregate
        {
            return _snapshots.Cast<SnapshotElement<T>>().ToList();
        }

        private static SnapshotElement<T> Copy<T>(SnapshotElement<BaseAggregate> obj) where T : BaseAggregate
        {
            var copy = (T)(obj.Content).Clone();
            return new SnapshotElement<T>(obj.Start, obj.CreateDateTime, obj.Id, copy);
        }
    }
}
