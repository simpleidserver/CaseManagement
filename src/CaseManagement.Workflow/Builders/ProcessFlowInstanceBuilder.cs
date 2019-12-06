using CaseManagement.Workflow.Domains;
using System.Collections.Generic;

namespace CaseManagement.Workflow.Builders
{
    public class ProcessFlowInstanceBuilder
    {
        protected ProcessFlowInstanceBuilder()
        {

        }

        protected ProcessFlowInstanceBuilder(string processFlowTemplateId, string processFlowName)
        {
            ProcessFlowTemplateId = processFlowTemplateId;
            ProcessFlowName = processFlowName;
            Elements = new List<ProcessFlowInstanceElement>();
            Connectors = new List<ProcessFlowConnector>();
        }

        protected string ProcessFlowTemplateId { get; set; }
        protected string ProcessFlowName { get; set; }
        protected ICollection<ProcessFlowInstanceElement> Elements { get; set; }
        protected ICollection<ProcessFlowConnector> Connectors { get; set; }

        public static ProcessFlowInstanceBuilder New(string processFlowTemplateId, string processFlowName)
        {
            return new ProcessFlowInstanceBuilder(processFlowTemplateId, processFlowName);
        }

        public virtual ProcessFlowInstanceBuilder AddElement(ProcessFlowInstanceElement node)
        {
            Elements.Add(node);
            return this;
        }

        public ProcessFlowInstanceBuilder AddConnection(string sourceNodeId, string targetNodeId)
        {
            Connectors.Add(new ProcessFlowConnector(sourceNodeId, targetNodeId));
            return this;
        }

        public virtual ProcessFlowInstance Build()
        {
            return ProcessFlowInstance.New(ProcessFlowTemplateId, ProcessFlowName, Elements, Connectors);
        }
    }
}
