using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.Bus;
using CaseManagement.CMMN.Infrastructure.EvtStore;
using CaseManagement.CMMN.Infrastructure.Jobs.Notifications;
using CaseManagement.CMMN.Infrastructure.Lock;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.Jobs
{
    public class ProcessCasePlanInstanceJob : BaseJob<CasePlanInstanceNotification>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICasePlanInstanceProcessor _casePlanInstanceProcessor;
        private readonly IDistributedLock _distributedLock;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public ProcessCasePlanInstanceJob(IEventStoreRepository eventStoreRepository, IMessageBroker messageBroker, IOptions<CMMNServerOptions> options, ICasePlanInstanceProcessor casePlanInstanceProcessor, IDistributedLock distributedLock, ICommitAggregateHelper commitAggregateHelper) : base(messageBroker, options)
        {
            _eventStoreRepository = eventStoreRepository;
            _casePlanInstanceProcessor = casePlanInstanceProcessor;
            _distributedLock = distributedLock;
            _commitAggregateHelper = commitAggregateHelper;
        }

        protected override string QueueName => CMMNConstants.QueueNames.CasePlanInstances;

        protected override async Task ProcessMessage(CasePlanInstanceNotification notification, CancellationToken cancellationToken)
        {
            string lockName = $"caseplaninstance:{notification.Id}";
            if (!await _distributedLock.TryAcquireLock(lockName, cancellationToken))
            {
                return;
            }

            var casePlanInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(notification.CasePlanInstanceId, CasePlanInstanceAggregate.GetStreamName(notification.CasePlanInstanceId));
            try
            {
                if (casePlanInstance == null || string.IsNullOrWhiteSpace(casePlanInstance.AggregateId))
                {
                    throw new InvalidOperationException($"case plan instance '{notification.CasePlanInstanceId}' doesn't exist");
                }

                await _casePlanInstanceProcessor.Execute(casePlanInstance, cancellationToken);
                await _commitAggregateHelper.Commit(casePlanInstance, casePlanInstance.GetStreamName(), cancellationToken);
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockName, cancellationToken);
            }
        }
    }
}
