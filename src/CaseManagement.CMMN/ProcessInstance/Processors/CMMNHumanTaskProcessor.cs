using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.ProcessInstance.Processors
{
    public class CMMNHumanTaskProcessor : IProcessFlowElementProcessor
    {
        public Type ProcessFlowElementType => typeof(CMMNHumanTask);

        public Task Handle(ProcessFlowInstanceElement pfe, ProcessFlowInstanceExecutionContext context)
        {
            var humanTask = (CMMNHumanTask)pfe;
            if (humanTask.IsBlocking)
            {
                return Task.FromResult(0);
            }

            humanTask.Finish();
            return Task.FromResult(0);
        }
    }
}
