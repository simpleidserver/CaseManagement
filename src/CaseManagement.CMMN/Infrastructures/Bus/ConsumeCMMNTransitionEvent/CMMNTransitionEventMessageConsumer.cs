using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus.ConsumeCMMNTransitionEvent
{
    public class CMMNTransitionEventMessageConsumer : BaseMessageConsumer
    {
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;

        public CMMNTransitionEventMessageConsumer(ILogger<CMMNTransitionEventMessageConsumer> logger, IDistributedLock distributedLock, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(taskPool, queueProvider, options)
        {
            _logger = logger;
            _distributedLock = distributedLock;
        }

        public override string QueueName => QUEUE_NAME;
        public const string QUEUE_NAME = "received-event";

        protected override async Task<RunningTask> Execute(string queueMessage)
        {
            var message = JsonConvert.DeserializeObject<CMMNTransitionEventMessage>(queueMessage);
            var lockId = $"{QueueName}-{message.Id}";
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
                    var workflowInstance = runningTask.Aggregate as CMMNWorkflowInstance;
                    try
                    {
                        if (string.IsNullOrWhiteSpace(message.CaseInstanceElementId))
                        {
                            workflowInstance.MakeTransition(message.Transition);
                        }
                        else if (workflowInstance.WorkflowElementInstances.Any(i => i.Id == message.CaseInstanceElementId))
                        {
                            workflowInstance.MakeTransition(message.CaseInstanceElementId, message.Transition);
                        }
                    }
                    catch(AggregateValidationException)
                    {
                        // TODO : AJOUTER LOG.
                    }
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