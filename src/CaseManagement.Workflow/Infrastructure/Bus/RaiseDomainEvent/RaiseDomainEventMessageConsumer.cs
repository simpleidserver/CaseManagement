using CaseManagement.Workflow.Infrastructure.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus.RaiseDomainEvent
{
    public class RaiseDomainEventMessageConsumer : BaseMessageConsumer
    {
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;

        public RaiseDomainEventMessageConsumer(ILogger<RaiseDomainEventMessageConsumer> logger, IDistributedLock distributedLock, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(taskPool, queueProvider, options)
        {
            _logger = logger;
            _distributedLock = distributedLock;
        }

        public override string QueueName => QUEUE_NAME;
        public const string QUEUE_NAME = "received-event";

        protected override async Task<RunningTask> Execute(string queueMessage)
        {
            var message = JsonConvert.DeserializeObject<RaiseDomainEventMessage>(queueMessage);
            var domainEvent = JsonConvert.DeserializeObject(message.Content, Type.GetType(message.AssemblyQualifiedName));
            Debug.WriteLine($"Event is received process {queueMessage}");
            var lockId = $"{QueueName}-{((DomainEvent)domainEvent).Id}";
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
                    ((DomainEvent)domainEvent).Version = runningTask.ProcessFlowInstance.Version + 1;
                    runningTask.ProcessFlowInstance.DomainEvents.Add((DomainEvent)domainEvent);
                    runningTask.ProcessFlowInstance.Handle(domainEvent);
                }
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockId);
            }

            return null;
        }
    }
}
