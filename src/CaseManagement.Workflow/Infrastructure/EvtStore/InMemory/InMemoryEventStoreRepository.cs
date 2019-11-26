using NEventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.EvtStore.InMemory
{
    public class EventStoreRepository<T> : IEventStoreRepository<T> where T : BaseAggregate
    {
        private readonly IAggregateSnapshotStore<T> _aggregateSnapshotStore;
        private readonly IStoreEvents _storeEvents;

        public EventStoreRepository(IAggregateSnapshotStore<T> aggregateSnapshotStore, IStoreEvents storeEvents)
        {
            _aggregateSnapshotStore = aggregateSnapshotStore;
            _storeEvents = storeEvents;
        }

        public async Task<T> GetLastAggregate(string id, string streamName)
        {
            var domainEvents = await GetLastDomainEvents(id, streamName);
            return await GetLastAggregate(id, domainEvents);
        }

        public async Task<T> GetLastAggregate(string id, IEnumerable<DomainEvent> domainEvents)
        {
            var snapCv = await _aggregateSnapshotStore.GetLast(id);
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

        public async Task<IEnumerable<DomainEvent>> GetLastDomainEvents(string id, string streamName)
        {
            var snapCv = await _aggregateSnapshotStore.GetLast(id);
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
