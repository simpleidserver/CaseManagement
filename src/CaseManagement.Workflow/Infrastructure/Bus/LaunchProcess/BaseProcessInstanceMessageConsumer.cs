using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Infrastructure.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
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

        protected override Task<RunningTask> Execute(string queueMessage)
        {
            var message = JsonConvert.DeserializeObject<LaunchProcessMessage>(queueMessage);
            var cancellationTokenSource = new CancellationTokenSource();
            var task = new Task(async () => await HandleLaunchProcess(message.ProcessFlowId, cancellationTokenSource.Token));
            return Task.FromResult(new RunningTask(message.ProcessFlowId, task, cancellationTokenSource));
        }

        private async Task HandleLaunchProcess(string pfId, CancellationToken token)
        {
            var flowInstance = await _eventStoreRepository.GetLastAggregate<T>(pfId, GetStreamName(pfId));
            if (flowInstance == null)
            {
                TaskPool.RemoveTask(flowInstance.Id);
                return;
            }

            var lockId = flowInstance.Id;
            if (!await _distributedLock.AcquireLock(lockId))
            {
                _logger.LogDebug($"The process flow {lockId} is locked !");
                await Task.Delay(Options.ConcurrencyExceptionIdleTimeInMs);
                await HandleLaunchProcess(pfId, token);
                return;
            }

            try
            {
                try
                {
                    flowInstance.Launch();
                    await _workflowEngine.Start(flowInstance, token);
                }
                finally
                {
                    if (flowInstance.IsFinished())
                    {
                        flowInstance.Complete();
                    }

                    await _commitAggregateHelper.Commit(flowInstance, flowInstance.GetStreamName());
                }
            }
            finally
            {
                TaskPool.RemoveTask(flowInstance.Id);
                await _distributedLock.ReleaseLock(lockId);
            }
        }

        protected abstract string GetStreamName(string id);
    }
}
