using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.CMMN.Infrastructures.Lock;
using CaseManagement.CMMN.Persistence;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus.ReactivateProcess
{
    public class ReactivateProcessCommandHandler : IMessageBrokerConsumerGeneric<ReactivateProcessCommand>
    {
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;
        private readonly ICaseEngine _workflowEngine;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICasePlanQueryRepository _casePlanQueryRepository;
        private readonly IRunningTaskPool _runningTaskPool;

        public ReactivateProcessCommandHandler(ILogger<ReactivateProcessCommandHandler> logger, IDistributedLock distributedLock, ICaseEngine workflowEngine, ICommitAggregateHelper commitAggregateHelper, IEventStoreRepository eventStoreRepository, ICasePlanQueryRepository casePlanQueryRepository, IRunningTaskPool runningTaskPool)
        {
            _logger = logger;
            _distributedLock = distributedLock;
            _workflowEngine = workflowEngine;
            _commitAggregateHelper = commitAggregateHelper;
            _eventStoreRepository = eventStoreRepository;
            _casePlanQueryRepository = casePlanQueryRepository;
            _runningTaskPool = runningTaskPool;
        }

        public string QueueName => CMMNConstants.QueueNames.CasePlanInstances;

        public async Task Handle(ReactivateProcessCommand command, CancellationToken token)
        {
            var lockId = $"caseplaninstance-{command.CasePlanInstanceId}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                _logger.LogDebug($"The command '{lockId}' is locked !");
                return;
            }
            
            var workflowInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(command.CasePlanInstanceId, CasePlanInstanceAggregate.GetStreamName(command.CasePlanInstanceId));
            var workflowDefinition = await _casePlanQueryRepository.FindById(workflowInstance.CasePlanId);
            var task = new Task(async () => await HandleLaunchProcess(workflowDefinition, workflowInstance, token, lockId), TaskCreationOptions.LongRunning);
            task.Start();
            var runningTask = new RunningTask(command.CasePlanInstanceId, task, workflowInstance);
            _runningTaskPool.AddTask(runningTask);
        }

        private async Task HandleLaunchProcess(CasePlanAggregate casePlan, CasePlanInstanceAggregate casePlanInstance, CancellationToken token, string lockId)
        {
            try
            {
                try
                {
                    casePlanInstance.EventRaised += HandleEventRaised;
                    await _workflowEngine.Reactivate(casePlan, casePlanInstance, token);
                    token.ThrowIfCancellationRequested();
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