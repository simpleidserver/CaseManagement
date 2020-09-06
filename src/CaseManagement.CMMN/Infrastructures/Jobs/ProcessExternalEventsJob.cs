using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.ExternalEvts;
using CaseManagement.CMMN.Infrastructures.Jobs;
using CaseManagement.CMMN.Infrastructures.Jobs.Notifications;
using CaseManagement.CMMN.Infrastructures.Lock;
using CaseManagement.Workflow.Infrastructure.Bus;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.BackgroundTasks
{
    public class ProcessExternalEventsJob : BaseJob<ExternalEventNotification>
    {
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IDistributedLock _distributedLock;

        public ProcessExternalEventsJob(IMessageBroker messageBroker, IOptions<CMMNServerOptions> options, ISubscriberRepository subscriberRepository, IDistributedLock distributedLock) : base(messageBroker, options)
        {
            _subscriberRepository = subscriberRepository;
            _distributedLock = distributedLock;
        }

        protected override string QueueName => CMMNConstants.QueueNames.ExternalEvents;

        protected override async Task ProcessMessage(ExternalEventNotification notification, CancellationToken cancellationToken)
        {
            string lockName = $"extevt:{notification.Id}";
            if (!await _distributedLock.TryAcquireLock(lockName, cancellationToken))
            {
                return;
            }

            try
            {
                var subscriber = await _subscriberRepository.Get(notification.CasePlanInstanceId, notification.CasePlanElementInstanceId, notification.EvtName, cancellationToken);
                if (subscriber == null)
                {
                    throw new InvalidOperationException("subscriber doesn't exist");
                }

                subscriber.IsCaptured = true;
                await _subscriberRepository.Update(subscriber, cancellationToken);
                await MessageBroker.QueueCasePlanInstance(notification.CasePlanInstanceId);
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockName, cancellationToken);
            }
        }
    }
}
