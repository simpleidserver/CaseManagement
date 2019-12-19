using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Infrastructure.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus.LaunchProcess
{
    public class CMMNLaunchProcessMessageConsumer : BaseMessageConsumer
    {
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;
        private readonly ICMMNWorkflowEngine _workflowEngine;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IEventStoreRepository _eventStoreRepository;

        public CMMNLaunchProcessMessageConsumer(ILogger<CMMNLaunchProcessMessageConsumer> logger, IDistributedLock distributedLock, ICMMNWorkflowEngine workflowEngine, ICommitAggregateHelper commitAggregateHelper, IEventStoreRepository eventStoreRepository, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(taskPool, queueProvider, options)
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

            // var flowInstance = await _eventStoreRepository.GetLastAggregate<CMMNWorkflowInstance>(message.ProcessFlowId, null/*GetStreamName(message.ProcessFlowId)*/);
            // var task = new Task(async () => await HandleLaunchProcess(flowInstance, message.ProcessFlowId, cancellationTokenSource.Token));
            // return new RunningTask(message.ProcessFlowId, task, flowInstance, cancellationTokenSource);
            return null;
        }

        private async Task HandleLaunchProcess(CMMNWorkflowInstance flowInstance, string taskId, CancellationToken token)
        {
            var lockId = flowInstance.Id;
            Debug.WriteLine($"Launch process {lockId}");
            try
            {
                try
                {
                    flowInstance.EventRaised += HandleEventRaised;
                    // await _workflowEngine.Start(flowInstance, token);
                    token.ThrowIfCancellationRequested();
                }
                catch(OperationCanceledException)
                {
                    // flowInstance.Cancel();
                }
                finally
                {
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
            // await _commitAggregateHelper.Commit(e.ProcessFlowInstance, e.ProcessFlowInstance.GetStreamName());
        }

        // protected abstract string GetStreamName(string id);
    }
}
