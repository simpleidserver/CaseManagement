using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Infrastructure.Lock;
using CaseManagement.Workflow.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus.ConfirmForm
{
    public class ConfirmFormConsumer : BaseMessageConsumer
    {
        private readonly IFormQueryRepository _formQueryRepository;
        private readonly ILogger _logger;
        private readonly IDistributedLock _distributedLock;
        private readonly IWorkflowEngine _workflowEngine;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IEventStoreRepository _eventStoreRepository;

        public ConfirmFormConsumer(IFormQueryRepository formQueryRepository, ILogger<ConfirmFormConsumer> logger, IDistributedLock distributedLock, IWorkflowEngine workflowEngine, ICommitAggregateHelper commitAggregateHelper, IEventStoreRepository eventStoreRepository, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(taskPool, queueProvider, options)
        {
            _formQueryRepository = formQueryRepository;
            _logger = logger;
            _distributedLock = distributedLock;
            _workflowEngine = workflowEngine;
            _commitAggregateHelper = commitAggregateHelper;
            _eventStoreRepository = eventStoreRepository;
        }

        public override string QueueName => QUEUE_NAME;
        public const string QUEUE_NAME = "confirm-form";

        protected override Task<RunningTask> Execute(string queueMessage)
        {
            var message = JsonConvert.DeserializeObject<ConfirmFormMessage>(queueMessage);
            var cancellationTokenSource = new CancellationTokenSource();
            var processId = $"{QUEUE_NAME}-{message.ProcessElementId}-{message.ProcessFlowId}";
            var task = new Task(async () => await HandleConfirmForm(message, processId, cancellationTokenSource.Token));
            return Task.FromResult(new RunningTask(processId, task, cancellationTokenSource));
        }

        private async Task HandleConfirmForm(ConfirmFormMessage confirmFormMessage, string processId, CancellationToken cancellationToken)
        {
            var flowInstance = await _eventStoreRepository.GetLastAggregate<CMMNProcessFlowInstance>(confirmFormMessage.ProcessFlowId, CMMNProcessFlowInstance.GetCMMNStreamName(confirmFormMessage.ProcessFlowId));
            if (flowInstance == null)
            {
                TaskPool.RemoveTask(processId);
                return;
            }
            
            var lockId = flowInstance.Id;
            if (!await _distributedLock.AcquireLock(lockId))
            {
                _logger.LogDebug($"The process flow {lockId} is locked !");
                await Task.Delay(Options.ConcurrencyExceptionIdleTimeInMs);
                await HandleConfirmForm(confirmFormMessage, processId, cancellationToken);
                return;
            }

            try
            {
                try
                {
                    var flowInstanceElt = flowInstance.Elements.FirstOrDefault(e => e.Id == confirmFormMessage.ProcessElementId) as CMMNPlanItem;
                    var humanTask = flowInstanceElt.PlanItemDefinitionHumanTask;
                    var form = await _formQueryRepository.FindFormById(humanTask.FormId);
                    flowInstance.ConfirmForm(confirmFormMessage.ProcessElementId, form, confirmFormMessage.Content);
                    await _workflowEngine.Start(flowInstance, cancellationToken);
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
                TaskPool.RemoveTask(processId);
                await _distributedLock.ReleaseLock(lockId);
            }
        }
    }
}
