using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Infrastructure.Jobs.Notifications;
using CaseManagement.BPMN.ProcessInstance.Processors;
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
    public class ProcessInstanceJob : BaseJob<ProcessInstanceNotification>
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IProcessInstanceProcessor _processInstanceProcessor;
        private readonly IDistributedLock _distributedLock;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public ProcessInstanceJob(IEventStoreRepository eventStoreRepository, IProcessInstanceProcessor processInstanceProcessor, IDistributedLock distributedLock, ICommitAggregateHelper commitAggregateHelper, IMessageBroker messageBroker, IOptions<CommonOptions> options) : base(messageBroker, options)
        {
            _eventStoreRepository = eventStoreRepository;
            _processInstanceProcessor = processInstanceProcessor;
            _distributedLock = distributedLock;
            _commitAggregateHelper = commitAggregateHelper;
        }

        protected override string QueueName => BPMNConstants.QueueNames.ProcessInstances;

        protected override async Task ProcessMessage(ProcessInstanceNotification notification, CancellationToken cancellationToken)
        {
            string lockName = $"processinstances:{notification.Id}";
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

                await _processInstanceProcessor.Execute(processInstance, cancellationToken);
                await _commitAggregateHelper.Commit(processInstance, processInstance.GetStreamName(), cancellationToken);
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockName, cancellationToken);
            }
        }
    }
}
