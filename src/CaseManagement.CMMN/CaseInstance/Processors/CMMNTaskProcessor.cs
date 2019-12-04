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
        public override Type ProcessFlowElementType => typeof(CMMNTask);

        public override async Task Run(WorkflowHandlerContext context, CancellationToken token)
        {
            context.ProcessFlowInstance.CompletePlanItem(context.GetCMMNPlanItem());
            await context.Complete(token);
        }
    }
}
