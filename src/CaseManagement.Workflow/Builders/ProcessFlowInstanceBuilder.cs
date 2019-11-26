using CaseManagement.Workflow.Domains;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Builders
{
    public class ProcessFlowInstanceBuilder
    {
        private ICollection<ProcessFlowInstanceElement> _elements;
        private ICollection<ProcessFlowConnector> _connectors;

        private ProcessFlowInstanceBuilder()
        {
            _elements = new List<ProcessFlowInstanceElement>();
            _connectors = new List<ProcessFlowConnector>();
        }

        public static ProcessFlowInstanceBuilder New()
        {
            return new ProcessFlowInstanceBuilder();
        }

        public ProcessFlowInstanceBuilder AddElement(ProcessFlowInstanceElement node)
        {
            _elements.Add(node);
            return this;
        }

        public ProcessFlowInstanceBuilder AddConnection(string sourceNodeId, string targetNodeId)
        {
            _connectors.Add(new ProcessFlowConnector(_elements.First(e => e.Id == sourceNodeId), _elements.First(e => e.Id == targetNodeId)));
            return this;
        }

        public ProcessFlowInstance Build()
        {
            return ProcessFlowInstance.New(_elements, _connectors);
        }
    }
}
