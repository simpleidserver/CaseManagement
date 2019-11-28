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

        public Task Handle(ProcessFlowInstance pf, ProcessFlowInstanceElement pfe)
        {
            var receiveTask = pfe as BPMNReceiveTask;
            pf.LaunchElement(pfe);
            pf.CompleteElement(pfe);
            return Task.FromResult(0);
        }
    }
}
