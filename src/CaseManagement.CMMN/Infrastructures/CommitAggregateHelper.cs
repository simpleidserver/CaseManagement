using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using Microsoft.Extensions.Options;
using NEventStore;
using System;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public class CommitAggregateHelper : ICommitAggregateHelper
    {
        private readonly IStoreEvents _storeEvents;
        private readonly IQueueProvider _queueProvider;
        private readonly IAggregateSnapshotStore _aggregateSnapshotStore;
        private readonly SnapshotConfiguration _snapshotConfiguration;

        public CommitAggregateHelper(IStoreEvents storeEvents, IQueueProvider queueProvider, IAggregateSnapshotStore aggregateSnapshotStore, IOptions<SnapshotConfiguration> options)
        {
            _storeEvents = storeEvents;
            _queueProvider = queueProvider;
            _aggregateSnapshotStore = aggregateSnapshotStore;
            _snapshotConfiguration = options.Value;
        }

        public async Task Commit<T>(T aggregate, string streamName) where T : BaseAggregate
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
                // await _queueProvider.Queue(evt);
            }

            if (((aggregate.Version - 1) % _snapshotConfiguration.SnapshotFrequency) == 0)
            {
                await _aggregateSnapshotStore.Add(new SnapshotElement<BaseAggregate>(0, DateTime.UtcNow, aggregate));
            }
        }
    }
}
