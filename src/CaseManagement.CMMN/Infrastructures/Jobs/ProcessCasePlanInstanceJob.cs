using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.Jobs.Notifications;
using CaseManagement.CMMN.Infrastructures.Lock;
using CaseManagement.CMMN.Persistence;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Jobs
{
    public class ProcessCasePlanInstanceJob : BaseJob<CasePlanInstanceNotification>
    {
        private readonly ICasePlanInstanceProcessor _casePlanInstanceProcessor;
        private readonly ICasePlanInstanceQueryRepository _casePlanInstanceQueryRepository;
        private readonly IDistributedLock _distributedLock;

        public ProcessCasePlanInstanceJob(IMessageBroker messageBroker, IOptions<CMMNServerOptions> options, ICasePlanInstanceProcessor casePlanInstanceProcessor, ICasePlanInstanceQueryRepository casePlanInstanceQueryRepository, IDistributedLock distributedLock) : base(messageBroker, options)
        {
            _casePlanInstanceProcessor = casePlanInstanceProcessor;
            _casePlanInstanceQueryRepository = casePlanInstanceQueryRepository;
            _distributedLock = distributedLock;
        }

        protected override string QueueName => CMMNConstants.QueueNames.CasePlanInstances;

        protected override async Task ProcessMessage(CasePlanInstanceNotification notification, CancellationToken cancellationToken)
        {
            string lockName = $"caseplaninstance:{notification.Id}";
            if (!await _distributedLock.TryAcquireLock(lockName, cancellationToken))
            {
                return;
            }

            try
            {
                var casePlanInstance = await _casePlanInstanceQueryRepository.Get(notification.CasePlanInstanceId);
                await _casePlanInstanceProcessor.Execute(casePlanInstance, cancellationToken);
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockName, cancellationToken);
            }
        }
    }
}
