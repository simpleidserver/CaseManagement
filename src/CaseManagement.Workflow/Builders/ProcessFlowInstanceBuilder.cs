using CaseManagement.Workflow.Domains;

namespace CaseManagement.Workflow.Builders
{
    public class ProcessFlowInstanceBuilder
    {
        private readonly ProcessFlowInstance _instance;

        private ProcessFlowInstanceBuilder()
        {
            _instance = ProcessFlowInstance.New();
        }

        public static ProcessFlowInstanceBuilder New()
        {
            return new ProcessFlowInstanceBuilder();
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
