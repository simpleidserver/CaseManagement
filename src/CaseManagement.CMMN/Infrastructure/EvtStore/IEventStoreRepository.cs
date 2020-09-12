using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.EvtStore
{
    public interface IEventStoreRepository
    {
        Task<T> GetLastAggregate<T>(string id, string streamName) where T : BaseAggregate;
        Task<T> GetLastAggregate<T>(string id, string streamName, IEnumerable<DomainEvent> domainEvents) where T : BaseAggregate;
        Task<IEnumerable<DomainEvent>> GetLastDomainEvents<T>(string id, string streamName) where T : BaseAggregate;
    }
}
