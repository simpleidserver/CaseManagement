using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.EvtStore
{
    public interface IAggregateSnapshotStore<T> where T : BaseAggregate
    {
        Task<SnapshotElement<T>> GetLast(string id);
        Task<bool> Add(SnapshotElement<T> snapshot);
        ICollection<SnapshotElement<T>> Query();
    }
}
