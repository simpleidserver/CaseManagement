using CaseManagement.Workflow.Infrastructure.EvtBus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.EvtStore
{
    public interface IEventStoreRepository<T> where T : BaseAggregate
    {
        Task<T> GetLastAggregate(string id, string streamName);
        Task<T> GetLastAggregate(string id, IEnumerable<DomainEvent> domainEvents);
        Task<IEnumerable<DomainEvent>> GetLastDomainEvents(string id, string streamName);
    }
}
