using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.Workflow.Infrastructure.Bus;
using Microsoft.Extensions.Options;
using NEventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public class CommitAggregateHelper : ICommitAggregateHelper
    {
        private readonly IStoreEvents _storeEvents;
        private readonly IMessageBroker _messageBroker;
        private readonly IAggregateSnapshotStore _aggregateSnapshotStore;
        private readonly CMMNServerOptions _serverOptions;

        public CommitAggregateHelper(IStoreEvents storeEvents, IMessageBroker messageBroker, IAggregateSnapshotStore aggregateSnapshotStore, IOptions<CMMNServerOptions> options)
        {
            _storeEvents = storeEvents;
            _messageBroker = messageBroker;
            _aggregateSnapshotStore = aggregateSnapshotStore;
            _serverOptions = options.Value;
        }

        public async Task Commit<T>(T aggregate, string streamName, CancellationToken token) where T : BaseAggregate
        {
            using (var evtStream = _storeEvents.OpenStream(streamName, streamName, int.MinValue, int.MaxValue))
            {
                lock(aggregate.DomainEvents)
                {
                    foreach (var domainEvent in aggregate.DomainEvents)
                    {
                        evtStream.Add(new EventMessage { Body = domainEvent });
                    }
                }

                evtStream.CommitChanges(Guid.NewGuid());
            }

            await _messageBroker.QueueDomainEvents(aggregate.DomainEvents.ToList());
            if (((aggregate.Version - 1) % _serverOptions.SnapshotFrequency) == 0)
            {
                await _aggregateSnapshotStore.Add(new SnapshotElement<BaseAggregate>(0, DateTime.UtcNow, streamName, aggregate), token);
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

            await _messageBroker.QueueDomainEvents(evts);
            if (((aggregateVersion - 1) % _serverOptions.SnapshotFrequency) == 0)
            {
                await _aggregateSnapshotStore.Add(new SnapshotElement<BaseAggregate>(0, DateTime.UtcNow, streamName, aggregate), token);
            }
        }
    }
}
