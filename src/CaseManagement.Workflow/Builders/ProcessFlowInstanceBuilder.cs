using CaseManagement.Workflow.Domains;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Builders
{
    public class ProcessFlowInstanceBuilder
    {
        private string _processFlowTemplateId;
        private string _processFlowName;
        private ICollection<ProcessFlowInstanceElement> _elements;
        private ICollection<ProcessFlowConnector> _connectors;

        private ProcessFlowInstanceBuilder(string processFlowTemplateId, string processFlowName)
        {
            _processFlowTemplateId = processFlowTemplateId;
            _processFlowName = processFlowName;
            _elements = new List<ProcessFlowInstanceElement>();
            _connectors = new List<ProcessFlowConnector>();
        }

        public static ProcessFlowInstanceBuilder New(string processFlowTemplateId, string processFlowName)
        {
            return new ProcessFlowInstanceBuilder(processFlowTemplateId, processFlowName);
        }

        public ProcessFlowInstanceBuilder AddElement(ProcessFlowInstanceElement node)
        {
            _elements.Add(node);
            return this;
        }

        public ProcessFlowInstanceBuilder AddConnection(string sourceNodeId, string targetNodeId)
        {
            _connectors.Add(new ProcessFlowConnector(sourceNodeId, targetNodeId));
            return this;
        }

        public ProcessFlowInstance Build()
        {
            return ProcessFlowInstance.New(_processFlowTemplateId, _processFlowName, _elements, _connectors);
        }
    }
}
