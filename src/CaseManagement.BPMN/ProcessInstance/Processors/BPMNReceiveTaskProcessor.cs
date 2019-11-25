using CaseManagement.BPMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class BPMNReceiveTaskProcessor : IProcessFlowElementProcessor
    {
        public Type ProcessFlowElementType { get => typeof(BPMNReceiveTask); }

        public Task Handle(ProcessFlowInstanceElement pfe, ProcessFlowInstanceExecutionContext context)
        {
            var receiveTask = pfe as BPMNReceiveTask;
            receiveTask.Run();
            if (context.CallingOperation != receiveTask.OperationId)
            {
                return Task.FromResult(0);
            }

            receiveTask.Finish();
            return Task.FromResult(0);
        }
    }
}
