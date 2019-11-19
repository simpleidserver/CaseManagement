using CaseManagement.Workflow.Domains;

namespace CaseManagement.Workflow.Builders
{
    public class ProcessFlowInstanceBuilder
    {
        private readonly ProcessFlowInstance _instance;

        private ProcessFlowInstanceBuilder(ProcessFlowInstanceElement startElement)
        {
            _instance = new ProcessFlowInstance(startElement);
        }

        public static ProcessFlowInstanceBuilder New(ProcessFlowInstanceElement startElement)
        {
            return new ProcessFlowInstanceBuilder(startElement);
        }

        public ProcessFlowInstanceBuilder AddElement(ProcessFlowInstanceElement node)
        {
            _instance.AddElement(node);
            return this;
        }

        public ProcessFlowInstanceBuilder AddConnection(string sourceNodeId, string targetNodeId)
        {
            _instance.AddConnector(sourceNodeId, targetNodeId);
            return this;
        }

        public ProcessFlowInstance Build()
        {
            return _instance;
        }
    }
}
