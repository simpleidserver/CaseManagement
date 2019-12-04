using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Persistence;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus.LaunchProcess
{
    public class LaunchProcessMessageConsumer : BaseMessageConsumer
    {
        private readonly IProcessFlowInstanceQueryRepository _processFlowInstanceQueryRepository;
        private readonly IWorkflowEngine _workflowEngine;
        private readonly ICommitAggregateHelper _commitAggregateHelper;
        private readonly IEventStoreRepository _eventStoreRepository;

        public LaunchProcessMessageConsumer(IProcessFlowInstanceQueryRepository processFlowInstanceQueryRepository, IWorkflowEngine workflowEngine, ICommitAggregateHelper commitAggregateHelper, IEventStoreRepository eventStoreRepository, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(taskPool, queueProvider, options)
        {
            _processFlowInstanceQueryRepository = processFlowInstanceQueryRepository;
            _workflowEngine = workflowEngine;
            _commitAggregateHelper = commitAggregateHelper;
            _eventStoreRepository = eventStoreRepository;
        }

        public override string QueueName => QUEUE_NAME;
        public const string QUEUE_NAME = "launch-process";

        protected override async Task<RunningTask> Execute(string queueMessage)
        {
            var message = JsonConvert.DeserializeObject<LaunchProcessMessage>(queueMessage);
            var flowInstance = await _eventStoreRepository.GetLastAggregate<ProcessFlowInstance>(message.ProcessFlowId, ProcessFlowInstance.GetStreamName(message.ProcessFlowId));
            if (flowInstance == null)
            {
                return null;
            }

            var cancellationTokenSource = new CancellationTokenSource();
            var task = new Task(async (object data) =>
            {
                try
                {
                    var pf = (ProcessFlowInstance)data;
                    try
                    {
                        // TODO : BLOQUER LA RESSOURCE SI ELLE EST DEJA EN COURS D EXECUTION.
                        await _workflowEngine.Start(pf, cancellationTokenSource.Token);
                    }
                    finally
                    {
                        if (pf.IsFinished())
                        {
                            pf.Complete();
                        }

                        await _commitAggregateHelper.Commit(pf, pf.GetStreamName());
                    }
                }
                finally
                {
                    TaskPool.RemoveTask(message.ProcessFlowId);
                }
            }, flowInstance, cancellationTokenSource.Token);
            return new RunningTask(message.ProcessFlowId, task, cancellationTokenSource);
        }
    }
}
