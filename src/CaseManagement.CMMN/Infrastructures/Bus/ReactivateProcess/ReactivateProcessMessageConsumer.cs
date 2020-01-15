using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Infrastructure.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus.ReactivateProcess
{
    public class ReactivateProcessMessageConsumer : BaseMessageConsumer
    {
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;
        private readonly ICaseEngine _workflowEngine;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICaseDefinitionQueryRepository _cmmnWorkflowDefinitionQueryRepository;

        public ReactivateProcessMessageConsumer(ILogger<ReactivateProcessMessageConsumer> logger, IDistributedLock distributedLock, ICaseEngine workflowEngine, ICommitAggregateHelper commitAggregateHelper, IEventStoreRepository eventStoreRepository, ICaseDefinitionQueryRepository cmmnWorkflowDefinitionQueryRepository, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(taskPool, queueProvider, options)
        {
            _logger = logger;
            _distributedLock = distributedLock;
            _workflowEngine = workflowEngine;
            _commitAggregateHelper = commitAggregateHelper;
            _eventStoreRepository = eventStoreRepository;
            _cmmnWorkflowDefinitionQueryRepository = cmmnWorkflowDefinitionQueryRepository;
        }

        public override string QueueName => QUEUE_NAME;
        public const string QUEUE_NAME = "reactivate-process";

        protected override async Task<RunningTask> Execute(string queueMessage)
        {
            var message = JsonConvert.DeserializeObject<ReactivateProcessMessage>(queueMessage);
            var cancellationTokenSource = new CancellationTokenSource();
            var lockId = message.CaseInstanceId;
            if (!await _distributedLock.AcquireLock(lockId))
            {
                _logger.LogDebug($"The process flow {lockId} is locked !");
                return null;
            }
            
            await QueueProvider.Dequeue(QueueName);
            var workflowInstance = await _eventStoreRepository.GetLastAggregate<Domains.CaseInstance>(message.CaseInstanceId, Domains.CaseInstance.GetStreamName(message.CaseInstanceId));
            var workflowDefinition = await _cmmnWorkflowDefinitionQueryRepository.FindById(workflowInstance.CaseDefinitionId);
            var task = new Task(async () => await HandleLaunchProcess(workflowDefinition, workflowInstance, message.CaseInstanceId, lockId, cancellationTokenSource.Token));
            return new RunningTask(message.CaseInstanceId, task, workflowInstance, cancellationTokenSource);
        }

        private async Task HandleLaunchProcess(CaseDefinition workflowDefinition, Domains.CaseInstance workflowInstance, string taskId, string lockId, CancellationToken token)
        {
            Debug.WriteLine($"Reactivate process {lockId}");
            try
            {
                try
                {
                    workflowInstance.EventRaised += HandleEventRaised;
                    await _workflowEngine.Reactivate(workflowDefinition, workflowInstance, token);
                    token.ThrowIfCancellationRequested();
                }
                finally
                {
                    workflowInstance.EventRaised -= HandleEventRaised;
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
            var workflowInstance = sender as Domains.CaseInstance;
            await _commitAggregateHelper.Commit(workflowInstance, new List<DomainEvent> { e.DomainEvt }, workflowInstance.Version, workflowInstance.GetStreamName());
        }
    }
}
