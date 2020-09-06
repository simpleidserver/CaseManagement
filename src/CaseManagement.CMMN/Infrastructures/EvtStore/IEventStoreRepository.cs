using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.EvtStore
{
    public interface IEventStoreRepository
    {
        Task<T> GetLastAggregate<T>(string id, string streamName, CancellationToken cancellationToken) where T : BaseAggregate;
        Task<T> GetLastAggregate<T>(string id, string streamName, IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken) where T : BaseAggregate;
        Task<IEnumerable<DomainEvent>> GetLastDomainEvents<T>(string id, string streamName, CancellationToken cancellationToken) where T : BaseAggregate;
    }
}
