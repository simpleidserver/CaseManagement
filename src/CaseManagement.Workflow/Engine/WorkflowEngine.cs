using CaseManagement.Workflow.Domains;
using System.Linq;
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

        public async Task Start(ProcessFlowInstance processFlowInstance)
        {
            bool result = false;
            foreach(var startElt in processFlowInstance.GetStartElements())
            {
                result = await Start(processFlowInstance, startElt);
            }

            if (result)
            {
                processFlowInstance.Complete();
            }
        }

        public async Task<bool> Start(ProcessFlowInstance processFlowInstance, ProcessFlowInstanceElement currentElt)
        {
            if (!processFlowInstance.CanStartElement(currentElt.Id))
            {
                return false;
            }

            if (!processFlowInstance.IsElementComplete(currentElt.Id))
            {
                var processor = _processFlowElementProcessorFactory.Build(currentElt);
                await processor.Handle(processFlowInstance, currentElt);
            }

            if (!processFlowInstance.IsElementComplete(currentElt.Id))
            {
                return false;
            }

            var nextElts = processFlowInstance.NextElements(currentElt.Id);
            if (!nextElts.Any())
            {
                return true;
            }

            bool result = true;
            foreach (var nextElt in nextElts)
            {
                if (!await Start(processFlowInstance, nextElt))
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
