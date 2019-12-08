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
    public class DomainEventMessageConsumer : IMessageConsumer
    {
        private readonly ILogger _logger;
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDistributedLock _distributedLock;
        private readonly BusOptions _options;
        private IQueueProvider _queueProvider;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _currentTask;

        public DomainEventMessageConsumer(ILogger<DomainEventMessageConsumer> logger, IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IServiceProvider serviceProvider, IDistributedLock distributedLock, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options)
        {
            _logger = logger;
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _serviceProvider = serviceProvider;
            _distributedLock = distributedLock;
            _options = options.Value;
            _queueProvider = queueProvider;
        }
        
        public string QueueName => QUEUE_NAME;
        public const string QUEUE_NAME = "event";

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _currentTask = new Task(Handle, TaskCreationOptions.LongRunning);
            _currentTask.Start();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _currentTask.Wait();
        }

        public virtual async void Handle()
        {
            var token = _cancellationTokenSource.Token;
            while (!token.IsCancellationRequested)
            {
                var queueMessage = await _queueProvider.Dequeue(QueueName);
                if (queueMessage == null)
                {
                    continue;
                }

                await Execute(queueMessage);
            }
        }

        private Task Execute(string queueMessage)
        {
            var domainEvtMessage = JsonConvert.DeserializeObject<DomainEventMessage>(queueMessage);
            var type = Type.GetType(domainEvtMessage.AssemblyQualifiedName);
            var concreteType = typeof(IDomainEventHandler<>).MakeGenericType(type);
            var evtHandler = _serviceProvider.GetService(concreteType);
            var domainEvt = JsonConvert.DeserializeObject(domainEvtMessage.Content, type);
            var castDomainEvt = (DomainEvent)domainEvt;
            return HandleDomainEvent(castDomainEvt, concreteType, evtHandler);
        }

        private async Task HandleDomainEvent(DomainEvent domainEvent, Type concreteType, object evtHandler)
        {
            var flowInstance = await _processFlowInstanceQueryRepository.FindFlowInstanceById(domainEvent.AggregateId);
            if ((flowInstance == null && domainEvent.Version > 0) || (flowInstance != null && (flowInstance.Version + 1) != domainEvent.Version))
            {
                Debug.WriteLine($"There is a concurrency error with the domain event, {domainEvent.GetType()} {domainEvent.Version} != {(flowInstance == null ? 0 : (flowInstance.Version + 1))} {JsonConvert.SerializeObject(domainEvent)}");
                _logger.LogDebug($"There is a concurrency error with the domain event, {domainEvent.Version} != {(flowInstance == null ? 0 : (flowInstance.Version + 1))}");
                await Task.Delay(_options.ConcurrencyExceptionIdleTimeInMs);
                await HandleDomainEvent(domainEvent, concreteType, evtHandler);
                return;
            }

            Debug.WriteLine($"Start event : {domainEvent.GetType()} {domainEvent.Version} {JsonConvert.SerializeObject(domainEvent)}");
            var lockId = $"{domainEvent.AggregateId}_{domainEvent.Version}";
            try
            {
                if (!await _distributedLock.AcquireLock(lockId))
                {
                    throw new ResourceLockException();
                }

                var task = (Task)concreteType.GetMethod("Handle").Invoke(evtHandler, new object[] { domainEvent, _cancellationTokenSource.Token });
                await task;
            }
            catch(ResourceLockException)
            {
                _logger.LogDebug($"Resource {lockId} is locked");
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockId);
            }
        }
    }
}