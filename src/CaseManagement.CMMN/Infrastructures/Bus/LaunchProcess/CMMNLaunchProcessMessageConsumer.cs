using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.Bus.LaunchProcess;
using CaseManagement.Workflow.Infrastructure.EvtStore;
using CaseManagement.Workflow.Infrastructure.Lock;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CaseManagement.CMMN.Infrastructures.Bus.LaunchProcess
{
    public class CMMNLaunchProcessMessageConsumer : BaseProcessInstanceMessageConsumer<CMMNProcessFlowInstance>
    {
        public CMMNLaunchProcessMessageConsumer(ILogger<BaseProcessInstanceMessageConsumer<CMMNProcessFlowInstance>> logger, IDistributedLock distributedLock, IWorkflowEngine workflowEngine, ICommitAggregateHelper commitAggregateHelper, IEventStoreRepository eventStoreRepository, IRunningTaskPool taskPool, IQueueProvider queueProvider, IOptions<BusOptions> options) : base(logger, distributedLock, workflowEngine, commitAggregateHelper, eventStoreRepository, taskPool, queueProvider, options)
        {
        }

        protected override string GetStreamName(string id)
        {
            return CMMNProcessFlowInstance.GetCMMNStreamName(id);
        }
    }
}
