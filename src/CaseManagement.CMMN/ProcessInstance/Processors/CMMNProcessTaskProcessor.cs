using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.ProcessInstance.Processors
{
    public class CMMNProcessTaskProcessor : CMMNPlanItemProcessor
    {
        public override Type ProcessFlowElementType => typeof(CMMNProcessTask);

        public override Task Handle(ProcessFlowInstanceElement pfe, ProcessFlowInstanceExecutionContext context)
        {
            var processTask = (CMMNProcessTask)pfe;
            processTask.Start();
            if (!CheckCanBeExecuted(processTask, context))
            {
                return Task.FromResult(0);
            }

            pfe.Finish();
            return Task.FromResult(0);
        }
    }
}
