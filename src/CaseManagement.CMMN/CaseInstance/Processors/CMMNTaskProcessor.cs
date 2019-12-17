using CaseManagement.CMMN.CaseInstance.Watchers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNTaskProcessor : BaseCMMNTaskProcessor
    {
        public CMMNTaskProcessor(IDomainEventWatcher domainEventWatcher, IProcessorHelper processorHelper) : base(domainEventWatcher, processorHelper)
        {
        }

        public override string ProcessFlowElementType => Enum.GetName(typeof(CMMNPlanItemDefinitionTypes), CMMNPlanItemDefinitionTypes.Task).ToLowerInvariant();
        
        public override async Task Run(WorkflowHandlerContext context, CancellationToken token)
        {
            context.ProcessFlowInstance.CompletePlanItem(context.GetCMMNPlanItem());
            await context.ExecuteNext(token);
        }
    }
}
