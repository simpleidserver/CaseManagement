using CaseManagement.Common.Bus;
using CaseManagement.Common.Domains;
using CaseManagement.Common.EvtStore;
using NEventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common
{
    public class CommitAggregateHelper : ICommitAggregateHelper
    {
        // TODO : EXTERNALIZE THIS PROPRERTY.
        private static object _obj = new object();
        private const int SNAP_SHOT_FREQUENCY = 200;
        private readonly IStoreEvents _storeEvents;
        private readonly IMessageBroker _messageBroker;
        private readonly IAggregateSnapshotStore _aggregateSnapshotStore;

        public CommitAggregateHelper(IStoreEvents storeEvents, IMessageBroker messageBroker, IAggregateSnapshotStore aggregateSnapshotStore)
        {
            _storeEvents = storeEvents;
            _messageBroker = messageBroker;
            _aggregateSnapshotStore = aggregateSnapshotStore;
        }

        public async Task Commit<T>(T aggregate, string streamName, CancellationToken cancellationToken) where T : BaseAggregate
        {
            var domainEvents = aggregate.DomainEvents.OrderBy(_ => _.Version);
            lock (_obj)
            {
                using (var evtStream = _storeEvents.OpenStream(streamName, streamName, int.MinValue, int.MaxValue))
                {
                    lock (domainEvents)
                    {
                        foreach (var domainEvent in domainEvents)
                        {
                            evtStream.Add(new EventMessage { Body = domainEvent });
                        }
                    }

                    evtStream.CommitChanges(Guid.NewGuid());
                }
            }

            await _messageBroker.QueueDomainEvents(domainEvents.ToList(), cancellationToken);
            if (aggregate.Version > 1 && (aggregate.Version - 1) % SNAP_SHOT_FREQUENCY == 0)
            {
                await _aggregateSnapshotStore.Add(new SnapshotElement<BaseAggregate>(0, DateTime.UtcNow, streamName, aggregate));
            }
        }

        public async Task Commit<T>(T aggregate, ICollection<DomainEvent> evts, int aggregateVersion, string streamName, CancellationToken token) where T : BaseAggregate
        {
            using (var evtStream = _storeEvents.OpenStream(streamName, streamName, int.MinValue, int.MaxValue))
            {
                foreach (var domainEvent in evts)
                {
                    evtStream.Add(new EventMessage { Body = domainEvent });
                }

                evtStream.CommitChanges(Guid.NewGuid());
            }

            await _messageBroker.QueueDomainEvents(evts, token);
            if (aggregate.Version > 1 && ((aggregateVersion - 1) % SNAP_SHOT_FREQUENCY) == 0)
            {
                await _aggregateSnapshotStore.Add(new SnapshotElement<BaseAggregate>(0, DateTime.UtcNow, streamName, aggregate));
            }
        }
    }
}
