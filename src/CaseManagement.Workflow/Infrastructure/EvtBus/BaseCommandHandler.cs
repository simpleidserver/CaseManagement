using CaseManagement.Workflow.Infrastructure.EvtStore;
using Microsoft.Extensions.Options;
using NEventStore;
using System;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.EvtBus
{
    public class BaseCommandHandler<T> where T : BaseAggregate
    {
        private readonly IStoreEvents _storeEvents;
        private readonly IEventBus _eventBus;
        private readonly IAggregateSnapshotStore<T> _aggregateSnapshotStore;
        private readonly SnapshotConfiguration _snapshotConfiguration;

        public BaseCommandHandler(IStoreEvents storeEvents, IEventBus eventBus, IAggregateSnapshotStore<T> aggregateSnapshotStore, IOptions<SnapshotConfiguration> options)
        {
            _storeEvents = storeEvents;
            _eventBus = eventBus;
            _aggregateSnapshotStore = aggregateSnapshotStore;
            _snapshotConfiguration = options.Value;
        }

        protected IStoreEvents StoreEvents => _storeEvents;
        protected IEventBus EventBus => _eventBus;
        protected IAggregateSnapshotStore<T> AggregateSnapshotStore => _aggregateSnapshotStore;
        protected SnapshotConfiguration SnapshotConfiguration => _snapshotConfiguration;

        protected async Task Commit(T aggregate, string streamName)
        {
            using (var evtStream = _storeEvents.OpenStream(streamName, streamName, int.MinValue, int.MaxValue))
            {
                foreach (var domainEvent in aggregate.DomainEvents)
                {
                    evtStream.Add(new EventMessage { Body = domainEvent });
                }

                evtStream.CommitChanges(Guid.NewGuid());
            }

            foreach (var evt in aggregate.DomainEvents)
            {
                _eventBus.Publish(evt);
            }

            if (((aggregate.Version - 1) % _snapshotConfiguration.SnapshotFrequency) == 0)
            {
                await _aggregateSnapshotStore.Add(new SnapshotElement<T>(0, DateTime.UtcNow, aggregate));
            }
        }
    }
}
