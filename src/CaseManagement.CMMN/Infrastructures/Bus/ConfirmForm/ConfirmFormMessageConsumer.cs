using CaseManagement.CMMN.Infrastructures.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus.ConfirmForm
{
    public class ConfirmFormMessageConsumer : BaseMessageConsumer
    {
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;

        public ConfirmFormMessageConsumer(ILogger<ConfirmFormMessageConsumer> logger, IDistributedLock distributedLock, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(taskPool, queueProvider, options)
        {
            _logger = logger;
            _distributedLock = distributedLock;
        }

        public override string QueueName => QUEUE_NAME;
        public const string QUEUE_NAME = "confirm-form";

        protected override async Task<RunningTask> Execute(string queueMessage)
        {
            var message = JsonConvert.DeserializeObject<ConfirmFormMessage>(queueMessage);
            var lockId = $"{QueueName}-{message.CaseInstanceId}-{message.CaseElementInstanceId}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                _logger.LogDebug($"The received event {lockId} is locked !");
                return null;
            }

            try
            {

                var runningTask = TaskPool.Get(message.CaseInstanceId);
                if (runningTask != null)
                {
                    await QueueProvider.Dequeue(QueueName);
                    var workflowInstance = runningTask.Aggregate as Domains.CaseInstance;
                    workflowInstance.SubmitForm(message.CaseElementInstanceId, message.FormInstanceId, message.FormValues);
                }

                return null;
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockId);
            }
        }
    }
}
