using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Infrastructure.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus.LaunchProcess
{
    public abstract class BaseProcessInstanceMessageConsumer<T> : BaseMessageConsumer where T : ProcessFlowInstance
    {
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;
        private readonly IWorkflowEngine _workflowEngine;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IEventStoreRepository _eventStoreRepository;

        public BaseProcessInstanceMessageConsumer(ILogger<BaseProcessInstanceMessageConsumer<T>> logger, IDistributedLock distributedLock, IWorkflowEngine workflowEngine, ICommitAggregateHelper commitAggregateHelper, IEventStoreRepository eventStoreRepository, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(taskPool, queueProvider, options)
        {
            _logger = logger;
            _distributedLock = distributedLock;
            _workflowEngine = workflowEngine;
            _commitAggregateHelper = commitAggregateHelper;
            _eventStoreRepository = eventStoreRepository;
        }

        public override string QueueName => QUEUE_NAME;
        public const string QUEUE_NAME = "launch-process";

        protected override async Task<RunningTask> Execute(string queueMessage)
        {
            var message = JsonConvert.DeserializeObject<LaunchProcessMessage>(queueMessage);
            var cancellationTokenSource = new CancellationTokenSource();
            var lockId = message.ProcessFlowId;
            await QueueProvider.Dequeue(QueueName);
            if (!await _distributedLock.AcquireLock(lockId))
            {
                _logger.LogDebug($"The process flow {lockId} is locked !");
                return null;
            }

            var flowInstance = await _eventStoreRepository.GetLastAggregate<T>(message.ProcessFlowId, GetStreamName(message.ProcessFlowId));
            var task = new Task(async () => await HandleLaunchProcess(flowInstance, message.ProcessFlowId, cancellationTokenSource.Token));
            return new RunningTask(message.ProcessFlowId, task, flowInstance, cancellationTokenSource);
        }

        private async Task HandleLaunchProcess(ProcessFlowInstance flowInstance, string taskId, CancellationToken token)
        {
            var lockId = flowInstance.Id;
            Debug.WriteLine($"Launch process {lockId}");
            try
            {
                try
                {
                    flowInstance.EventRaised += HandleEventRaised;
                    flowInstance.Launch();
                    await _workflowEngine.Start(flowInstance, token);
                    token.ThrowIfCancellationRequested();
                }
                catch(OperationCanceledException)
                {
                    flowInstance.Cancel();
                }
                finally
                {
                    if (flowInstance.IsFinished())
                    {
                        flowInstance.Complete();
                    }

                    flowInstance.EventRaised -= HandleEventRaised;
                }
            }
            finally
            {
                TaskPool.RemoveTask(taskId);
                await _distributedLock.ReleaseLock(lockId);
            }
        }

        private async void HandleEventRaised(object sender, DomainEventArgs e)
        {
            await _commitAggregateHelper.Commit(e.ProcessFlowInstance, e.ProcessFlowInstance.GetStreamName());
        }

        protected abstract string GetStreamName(string id);
    }
}
