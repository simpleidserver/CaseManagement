using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Infrastructure.Jobs.Notifications;
using CaseManagement.Common;
using CaseManagement.Common.Bus;
using CaseManagement.Common.EvtStore;
using CaseManagement.Common.Jobs;
using CaseManagement.Common.Lock;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Infrastructure.Jobs
{
    public class ProcessMessageJob : BaseJob<MessageNotification>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IDistributedLock _distributedLock;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public ProcessMessageJob(IEventStoreRepository eventStoreRepository, IDistributedLock distributedLock, ICommitAggregateHelper commitAggregateHelper, IMessageBroker messageBroker, IOptions<CommonOptions> options) : base(messageBroker, options)
        {
            _eventStoreRepository = eventStoreRepository;
            _distributedLock = distributedLock;
            _commitAggregateHelper = commitAggregateHelper;
        }

        protected override string QueueName => BPMNConstants.QueueNames.Messages;

        protected override async Task ProcessMessage(MessageNotification notification, CancellationToken cancellationToken)
        {
            string lockName = $"messages:{notification.Id}";
            if (!await _distributedLock.TryAcquireLock(lockName, cancellationToken))
            {
                return;
            }

            var processInstance = await _eventStoreRepository.GetLastAggregate<ProcessInstanceAggregate>(notification.ProcessInstanceId, ProcessInstanceAggregate.GetStreamName(notification.ProcessInstanceId));
            try
            {
                if (processInstance == null || string.IsNullOrWhiteSpace(processInstance.AggregateId))
                {
                    throw new InvalidOperationException($"process instance '{notification.ProcessInstanceId}' doesn't exist");
                }

                processInstance.ConsumeMessage(new MessageToken
                {
                    Name = notification.MessageName
                });
                await _commitAggregateHelper.Commit(processInstance, processInstance.GetStreamName(), cancellationToken);
                await MessageBroker.QueueProcessInstance(processInstance.AggregateId, false, cancellationToken);
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockName, cancellationToken);
            }
        }
    }
}
