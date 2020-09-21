using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.CMMN.Infrastructure.Jobs.Notifications;
using CaseManagement.Common;
using CaseManagement.Common.Bus;
using CaseManagement.Common.Jobs;
using CaseManagement.Common.Lock;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.Jobs
{
    public class ProcessExternalEventsJob : BaseJob<ExternalEventNotification>
    {
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IDistributedLock _distributedLock;

        public ProcessExternalEventsJob(IMessageBroker messageBroker, IOptions<CommonOptions> options, ISubscriberRepository subscriberRepository, IDistributedLock distributedLock) : base(messageBroker, options)
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
                await MessageBroker.QueueCasePlanInstance(notification.CasePlanInstanceId, cancellationToken);
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockName, cancellationToken);
            }
        }
    }
}
