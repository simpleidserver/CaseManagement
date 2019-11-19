using System;
using System.Threading.Tasks;
using CaseManagement.BPMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class BPMNEndEventProcessor : IProcessFlowElementProcessor
    {
        public Type ProcessFlowElementType => typeof(BPMNEndEvent);

        public Task Handle(ProcessFlowInstanceElement pfe, ProcessFlowInstanceExecutionContext context)
        {
            pfe.Finish();
            return Task.FromResult(0);
        }
    }
}
