using CaseManagement.BPMN.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class BPMNStartEventProcessor : IProcessFlowElementProcessor
    {
        public Type ProcessFlowElementType => typeof(BPMNStartEvent);

        public Task Handle(WorkflowHandlerContext context, CancellationToken token)
        {
            context.Start();
            context.Complete(token);
            return Task.FromResult(0);
        }
    }
}
