using CaseManagement.Workflow.Domains;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Engine
{
    public class ProcessFlowElementProcessorFactory : IProcessFlowElementProcessorFactory
    {
        private readonly IEnumerable<IProcessFlowElementProcessor> _processFlowElementProcessors;

        public ProcessFlowElementProcessorFactory(IEnumerable<IProcessFlowElementProcessor> processFlowElementProcessors)
        {
            _processFlowElementProcessors = processFlowElementProcessors;
        }

        public IProcessFlowElementProcessor Build(ProcessFlowInstanceElement elt)
        {
            var processor = _processFlowElementProcessors.First(p => p.ProcessFlowElementType == elt.ElementType);
            return processor;
        }
    }
}
