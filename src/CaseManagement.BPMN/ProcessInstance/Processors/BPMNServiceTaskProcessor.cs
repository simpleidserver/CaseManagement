using CaseManagement.BPMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using CaseManagement.Workflow.Infrastructure;
using System;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class BPMNServiceTaskProcessor : IProcessFlowElementProcessor
    {
        public Type ProcessFlowElementType => typeof(BPMNServiceTask);

        public async Task Handle(ProcessFlowInstance pf, ProcessFlowInstanceElement pfe)
        {
            pf.LaunchElement(pfe);
            var serviceTask = (BPMNServiceTask)pfe;
            var type = Type.GetType(serviceTask.FullQualifiedName);
            var instance = Activator.CreateInstance(type) as WorkflowTaskDelegate;
            await instance.Handle(pf);
            pf.CompleteElement(pfe);
        }
    }
}
