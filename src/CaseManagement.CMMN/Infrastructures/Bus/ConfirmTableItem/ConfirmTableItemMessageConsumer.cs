using CaseManagement.CMMN.Infrastructures.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus.ConfirmTableItem
{
    public class ConfirmTableItemMessageConsumer : BaseMessageConsumer
    {
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;

        public ConfirmTableItemMessageConsumer(ILogger<ConfirmTableItemMessageConsumer> logger, IDistributedLock distributedLock, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<CMMNServerOptions> options) : base(taskPool, queueProvider, options)
        {
            _logger = logger;
            _distributedLock = distributedLock;
        }

        public override string QueueName => QUEUE_NAME;
        public const string QUEUE_NAME = "confirm-tableitem";

        protected override async Task<RunningTask> Execute(string queueMessage)
        {
            var message = JsonConvert.DeserializeObject<ConfirmTableItemMessage>(queueMessage);
            var lockId = $"{QueueName}-{message.CaseInstanceId}-{message.CaseElementDefinitionId}";
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
                    workflowInstance.ConfirmTableItem(message.CaseElementDefinitionId, message.User);
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
