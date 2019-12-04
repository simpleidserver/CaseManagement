using CaseManagement.BPMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class BPMNEndEventProcessor : IProcessFlowElementProcessor
    {
        public Type ProcessFlowElementType => typeof(BPMNEndEvent);

        public Task Handle(WorkflowHandlerContext context, CancellationToken token)
        {
            context.Start();
            context.Complete(token);
            return Task.FromResult(0);
        }
    }
}
