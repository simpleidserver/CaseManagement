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

        private async Task<bool> Start(ProcessFlowInstance processFlowInstance, ProcessFlowInstanceElement elt)
        {
            var previousElts = processFlowInstance.PreviousElements(elt.Id);
            if (previousElts.Any(p => p.Status != ProcessFlowInstanceElementStatus.Finished))
            {
                return false;
            }

            if (elt.Status != ProcessFlowInstanceElementStatus.Finished)
            {
                var processor = _processFlowElementProcessorFactory.Build(elt);
                await processor.Handle(processFlowInstance, elt);
            }

            if (elt.Status != ProcessFlowInstanceElementStatus.Finished)
            {
                return false;
            }

            var nextElts = processFlowInstance.NextElements(elt.Id);
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
