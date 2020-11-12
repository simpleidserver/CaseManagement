using CaseManagement.BPMN.Common;
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Infrastructure.Jobs.Notifications;
using CaseManagement.Common;
using CaseManagement.Common.Bus;
using CaseManagement.Common.EvtStore;
using CaseManagement.Common.Jobs;
using CaseManagement.Common.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Infrastructure.Jobs
{
    public class ProcessStateTransitionJob : BaseJob<StateTransitionNotification>
    {
        private readonly ILogger<ProcessStateTransitionJob> _logger;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IDistributedLock _distributedLock;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public ProcessStateTransitionJob(
            ILogger<ProcessStateTransitionJob> logger,
            IEventStoreRepository eventStoreRepository,
            IDistributedLock distributedLock,
            ICommitAggregateHelper commitAggregateHelper,
            IMessageBroker messageBroker, 
            IOptions<CommonOptions> options) : base(messageBroker, options)
        {
            _logger = logger;
            _eventStoreRepository = eventStoreRepository;
            _distributedLock = distributedLock;
            _commitAggregateHelper = commitAggregateHelper;
        }

        protected override string QueueName => BPMNConstants.QueueNames.StateTransitions;

        protected override async Task ProcessMessage(StateTransitionNotification message, CancellationToken cancellationToken)
        {
            string lockName = $"statetransitions:{message.Id}";
            if (!await _distributedLock.TryAcquireLock(lockName, cancellationToken))
            {
                return;
            }

            var processInstance = await _eventStoreRepository.GetLastAggregate<ProcessInstanceAggregate>(message.ProcessInstanceId, ProcessInstanceAggregate.GetStreamName(message.ProcessInstanceId));
            try
            {
                if (processInstance == null || string.IsNullOrWhiteSpace(processInstance.AggregateId))
                {
                    throw new InvalidOperationException($"process instance '{message.ProcessInstanceId}' doesn't exist");
                }

                processInstance.ConsumeStateTransition(new StateTransitionToken
                {
                    State = message.State,
                    Content = message.Content,
                    FlowNodeInstanceId = message.FlowNodeInstanceId
                });
                await _commitAggregateHelper.Commit(processInstance, processInstance.GetStreamName(), cancellationToken);
                await MessageBroker.QueueProcessInstance(processInstance.AggregateId, false, cancellationToken);
                _logger.LogInformation($"Make transition '{message.State}' on the user task instance '{message.FlowNodeInstanceId}'");
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockName, cancellationToken);
            }
        }
    }
}
