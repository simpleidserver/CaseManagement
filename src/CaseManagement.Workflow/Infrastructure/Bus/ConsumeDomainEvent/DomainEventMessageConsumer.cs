using CaseManagement.Workflow.Infrastructure.Bus.Exceptions;
using CaseManagement.Workflow.Infrastructure.Lock;
using CaseManagement.Workflow.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus.ConsumeDomainEvent
{
    public class DomainEventMessageConsumer : BaseMessageConsumer
    {
        private readonly ILogger _logger;
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDistributedLock _distributedLock;

        public DomainEventMessageConsumer(ILogger<DomainEventMessageConsumer> logger, IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IServiceProvider serviceProvider, IDistributedLock distributedLock, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(taskPool, queueProvider, options)
        {
            _logger = logger;
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _serviceProvider = serviceProvider;
            _distributedLock = distributedLock;
        }

        public override string QueueName => QUEUE_NAME;
        public const string QUEUE_NAME = "event";

        protected override Task<RunningTask> Execute(string queueMessage)
        {
            var domainEvtMessage = JsonConvert.DeserializeObject<DomainEventMessage>(queueMessage);
            var type = Type.GetType(domainEvtMessage.AssemblyQualifiedName);
            var concreteType = typeof(IDomainEventHandler<>).MakeGenericType(type);
            var evtHandler = _serviceProvider.GetService(concreteType);
            var domainEvt = JsonConvert.DeserializeObject(domainEvtMessage.Content, type);
            var castDomainEvt = (DomainEvent)domainEvt;
            var cancellationTokenSource = new CancellationTokenSource();
            var task = new Task(async () => await HandleDomainEvent(castDomainEvt, concreteType, evtHandler, cancellationTokenSource.Token));
            return Task.FromResult(new RunningTask(castDomainEvt.Id, task, cancellationTokenSource));
        }

        private async Task HandleDomainEvent(DomainEvent domainEvent, Type concreteType, object evtHandler, CancellationToken token)
        {
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(domainEvent.AggregateId);
            if ((flowInstance == null && domainEvent.Version > 0) || (flowInstance != null && (flowInstance.Version + 1) != domainEvent.Version))
            {
                _logger.LogDebug($"There is a concurrency error with the domain event, {domainEvent.Version} != {(flowInstance == null ? 0 : (flowInstance.Version + 1))}");
                await Task.Delay(Options.ConcurrencyExceptionIdleTimeInMs);
                await HandleDomainEvent(domainEvent, concreteType, evtHandler, token);
                return;
            }

            var lockId = $"{domainEvent.AggregateId}_{domainEvent.Version}";
            try
            {
                if (!await _distributedLock.AcquireLock(lockId))
                {
                    throw new ResourceLockException();
                }

                concreteType.GetMethod("Handle").Invoke(evtHandler, new object[] { domainEvent, token });
            }
            catch(ResourceLockException)
            {
                _logger.LogDebug($"Resource {lockId} is locked");
            }
            finally
            {
                TaskPool.RemoveTask(domainEvent.Id);
                await _distributedLock.ReleaseLock(lockId);
            }
        }
    }
}