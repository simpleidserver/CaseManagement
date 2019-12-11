using CaseManagement.Workflow.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Engine
{
    public class WorkflowEngine : IWorkflowEngine
    {
        private readonly IProcessFlowElementProcessorFactory _processFlowElementProcessorFactory;

        public WorkflowEngine(IProcessFlowElementProcessorFactory processFlowElementProcessorFactory)
        {
            _processFlowElementProcessorFactory = processFlowElementProcessorFactory;
        }

        public async Task Start(ProcessFlowInstance processFlowInstance, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var taskLst = new List<Task>();
            foreach(var startElt in processFlowInstance.GetStartElements())
            {
                var handlerContext = new WorkflowHandlerContext(processFlowInstance, startElt, _processFlowElementProcessorFactory);
                taskLst.Add(Execute(handlerContext, cancellationToken));
            }

            await Task.WhenAll(taskLst);
        }

        private async Task Execute(WorkflowHandlerContext context, CancellationToken token)
        {
            var result = new List<Task>();
            await context.Execute(token);
            var subTasks = context.SubProcesses.Select(p => p.Task);
            await Task.WhenAll(subTasks);
        }
    }
}
