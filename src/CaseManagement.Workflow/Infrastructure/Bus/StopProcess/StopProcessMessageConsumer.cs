using CaseManagement.Workflow.Infrastructure.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus.StopProcess
{
    public class StopProcessMessageConsumer : BaseMessageConsumer
    {
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;

        public StopProcessMessageConsumer(ILogger<StopProcessMessageConsumer> logger, IDistributedLock distributedLock, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(taskPool, queueProvider, options)
        {
            _logger = logger;
            _distributedLock = distributedLock;
        }

        public override string QueueName => QUEUE_NAME;
        public const string QUEUE_NAME = "stop-process";

        protected override async Task<RunningTask> Execute(string queueMessage)
        {
            var message = JsonConvert.DeserializeObject<StopProcessMessage>(queueMessage);
            Debug.WriteLine($"Stop process {message.ProcessFlowId}");
            var lockId = $"{QueueName}-{message.ProcessFlowId}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                _logger.LogDebug($"The process flow {lockId} is locked !");
                return null;
            }

            await QueueProvider.Dequeue(QueueName);
            try
            {

                var runningTask = TaskPool.Get(message.ProcessFlowId);
                if (runningTask != null)
                {
                    runningTask.CancellationTokenSource.Cancel();
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
