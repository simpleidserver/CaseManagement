using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures.EvtStore;
using CaseManagement.CMMN.Infrastructures.Lock;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus.Transition
{
    public class TransitionCommandHandler : IMessageBrokerConsumerGeneric<TransitionCommand>
    {
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;
        private readonly IRunningTaskPool _runningTaskPool;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly ICommitAggregateHelper _commitAggregateHelper;

        public TransitionCommandHandler(ILogger<TransitionCommandHandler> logger, IDistributedLock distributedLock, IRunningTaskPool runningTaskPool, IEventStoreRepository eventStoreRepository, ICommitAggregateHelper commitAggregateHelper)
        {
            _logger = logger;
            _distributedLock = distributedLock;
            _runningTaskPool = runningTaskPool;
            _eventStoreRepository = eventStoreRepository;
            _commitAggregateHelper = commitAggregateHelper;
        }

        public string QueueName => CMMNConstants.QueueNames.CasePlanInstances;

        public async Task Handle(TransitionCommand command, CancellationToken token)
        {
            var lockId = $"transition-caseplaninstance-{command.Id}";
            if (!await _distributedLock.AcquireLock(lockId))
            {
                _logger.LogDebug($"The command '{lockId}' is locked !");
                return;
            }

            try
            {
                var runningTask = _runningTaskPool.Get(command.CasePlanInstanceId);
                CasePlanInstanceAggregate workflowInstance = null;
                bool commit = false;
                if (runningTask != null)
                {
                    workflowInstance = runningTask.Aggregate as CasePlanInstanceAggregate;
                }
                else
                {
                    commit = true;
                    workflowInstance = await _eventStoreRepository.GetLastAggregate<CasePlanInstanceAggregate>(command.CasePlanInstanceId, CasePlanInstanceAggregate.GetStreamName(command.CasePlanInstanceId));
                }

                try
                {
                    if (string.IsNullOrWhiteSpace(command.CasePlanElementInstanceId))
                    {
                        workflowInstance.MakeTransition(command.Transition);
                    }
                    else if (workflowInstance.WorkflowElementInstances.Any(i => i.Id == command.CasePlanElementInstanceId))
                    {
                        workflowInstance.MakeTransition(command.CasePlanElementInstanceId, command.Transition);
                    }

                    if (commit)
                    {
                        await _commitAggregateHelper.Commit(workflowInstance, CasePlanInstanceAggregate.GetStreamName(workflowInstance.Id), CMMNConstants.QueueNames.CasePlanInstances);
                    }
                }
                catch(AggregateValidationException ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockId);
            }
        }
    }
}