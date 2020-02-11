using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.CMMN.Infrastructures.Lock;
using CaseManagement.CMMN.Persistence;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus.LaunchCasePlanInstance
{
    public class LaunchCasePlanInstanceHandler : IMessageBrokerConsumerGeneric<LaunchCasePlanInstanceCommand>
    {
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;
        private readonly ICaseEngine _workflowEngine;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICasePlanQueryRepository _cmmnWorkflowDefinitionQueryRepository;
        private readonly IRunningTaskPool _runningTaskPool;

        public LaunchCasePlanInstanceHandler(ILogger<LaunchCasePlanInstanceHandler> logger, IDistributedLock distributedLock, ICaseEngine workflowEngine, ICommitAggregateHelper commitAggregateHelper, IEventStoreRepository eventStoreRepository, ICasePlanQueryRepository cmmnWorkflowDefinitionQueryRepository, IRunningTaskPool taskPool)
        {
            _logger = logger;
            _distributedLock = distributedLock;
            _workflowEngine = workflowEngine;
            _commitAggregateHelper = commitAggregateHelper;
            _eventStoreRepository = eventStoreRepository;
            _cmmnWorkflowDefinitionQueryRepository = cmmnWorkflowDefinitionQueryRepository;
            _runningTaskPool = taskPool;
        }

        public string QueueName => CMMNConstants.QueueNames.CasePlanInstances;

        public async Task Handle(LaunchCasePlanInstanceCommand message, CancellationToken token)
        {
            var lockId = $"caseplaninstance-{message.CasePlanInstanceId}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                _logger.LogDebug($"The command '{lockId}' is locked !");
                return;
            }

            var casePlanInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(message.CasePlanInstanceId, Domains.CasePlanInstanceAggregate.GetStreamName(message.CasePlanInstanceId));
            var casePlan = await _cmmnWorkflowDefinitionQueryRepository.FindById(casePlanInstance.CasePlanId);
            var task = new Task(async () => await HandleLaunchProcess(casePlan, casePlanInstance, token, lockId), TaskCreationOptions.LongRunning);
            task.Start();
            _runningTaskPool.AddTask(new RunningTask(message.CasePlanInstanceId, task, casePlanInstance));
        }

        private async Task HandleLaunchProcess(CasePlanAggregate casePlan, CasePlanInstanceAggregate casePlanInstance, CancellationToken token, string lockId)
        {
            Debug.WriteLine($"Launch process '{casePlanInstance.Id}'");
            try
            {
                try
                {
                    casePlanInstance.EventRaised += HandleEventRaised;
                    await _workflowEngine.Start(casePlan, casePlanInstance, token);
                }
                finally
                {
                    casePlanInstance.EventRaised -= HandleEventRaised;
                }
            }
            finally
            {
                _runningTaskPool.RemoveTask(casePlanInstance.Id);
                await _distributedLock.ReleaseLock(lockId);
            }
        }

        private async void HandleEventRaised(object sender, DomainEventArgs e)
        {
            var workflowInstance = sender as CasePlanInstanceAggregate;
            await _commitAggregateHelper.Commit(workflowInstance, new List<DomainEvent> { e.DomainEvt }, workflowInstance.Version, workflowInstance.GetStreamName(), CMMNConstants.QueueNames.CasePlanInstances);
        }
    }
}
