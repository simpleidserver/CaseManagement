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

        public TransitionCommandHandler(ILogger<TransitionCommandHandler> logger, IDistributedLock distributedLock, IRunningTaskPool runningTaskPool)
        {
            _logger = logger;
            _distributedLock = distributedLock;
            _runningTaskPool = runningTaskPool;
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
                if (runningTask != null)
                {
                    var workflowInstance = runningTask.Aggregate as Domains.CasePlanInstanceAggregate;
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
                    }
                    catch(AggregateValidationException ex)
                    {
                        _logger.LogError(ex.ToString());
                    }
                }
            }
            finally
            {
                await _distributedLock.ReleaseLock(lockId);
            }
        }
    }
}