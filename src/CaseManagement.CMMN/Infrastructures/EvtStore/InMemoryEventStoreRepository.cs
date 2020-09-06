using NEventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.EvtStore
{
    public class InMemoryEventStoreRepository : IEventStoreRepository
    {
        private readonly IAggregateSnapshotStore _aggregateSnapshotStore;
        private readonly IStoreEvents _storeEvents;

        public InMemoryEventStoreRepository(IAggregateSnapshotStore aggregateSnapshotStore, IStoreEvents storeEvents)
        {
            _aggregateSnapshotStore = aggregateSnapshotStore;
            _storeEvents = storeEvents;
        }

        public async Task<T> GetLastAggregate<T>(string id, string streamName, CancellationToken cancellationToken) where T : BaseAggregate
        {
            var domainEvents = await GetLastDomainEvents<T>(id, streamName, cancellationToken);
            return await GetLastAggregate<T>(id, streamName, domainEvents, cancellationToken);
        }

        public async Task<T> GetLastAggregate<T>(string id, string streamName, IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken) where T : BaseAggregate
        {
            var snapCv = await _aggregateSnapshotStore.GetLast<T>(streamName, cancellationToken);
            var instance = (T)Activator.CreateInstance(typeof(T));
            if (snapCv != null)
            {
                instance = (T)snapCv.Content;
            }

            foreach (var domainEvent in domainEvents)
            {
                instance.Handle(domainEvent);
            }

            return instance;
        }

        public async Task<IEnumerable<DomainEvent>> GetLastDomainEvents<T>(string id, string streamName, CancellationToken cancellationToken) where T : BaseAggregate
        {
            var snapCv = await _aggregateSnapshotStore.GetLast<T>(streamName, cancellationToken);
            DateTime? createDateTime = null;
            long position = 0;
            if (snapCv != null)
            {
                position = snapCv.Start + 1;
                createDateTime = snapCv.CreateDateTime;
            }

            var domainEvents = new List<DomainEvent>();
            if (createDateTime == null)
            {
                using (var stream = _storeEvents.OpenStream(streamName, streamName, 0, int.MaxValue))
                {
                    domainEvents.AddRange(stream.CommittedEvents.Select(e => (DomainEvent)e.Body));
                }
            }
            else
            {
                var commits = _storeEvents.Advanced.GetFrom(streamName, snapCv.CreateDateTime);
                foreach (var commit in commits)
                {
                    domainEvents.AddRange(commit.Events.Select(e => (DomainEvent)e.Body));
                }
            }
            
            return domainEvents;
        }
    }
}
