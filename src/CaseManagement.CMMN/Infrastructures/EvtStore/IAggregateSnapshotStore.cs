using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.EvtStore
{
    public interface IAggregateSnapshotStore 
    {
        Task<SnapshotElement<T>> GetLast<T>(string id, CancellationToken cancellationToken) where T : BaseAggregate;
        Task<bool> Add(SnapshotElement<BaseAggregate> snapshot, CancellationToken cancellationToken);
        ICollection<SnapshotElement<T>> Query<T>() where T : BaseAggregate;
    }
}
