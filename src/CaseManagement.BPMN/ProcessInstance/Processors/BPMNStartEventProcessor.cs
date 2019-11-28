using CaseManagement.BPMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class BPMNStartEventProcessor : IProcessFlowElementProcessor
    {
        public Type ProcessFlowElementType => typeof(BPMNStartEvent);

        public Task Handle(ProcessFlowInstance pf, ProcessFlowInstanceElement pfe)
        {
            pf.CompleteElement(pfe);
            return Task.FromResult(0);
        }
    }
}
