using CaseManagement.Workflow.Domains;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNServiceTask : ProcessFlowInstanceElement
    {
        public BPMNServiceTask(string id, string name) : base(id, name)
        {
        }

        public string FullQualifiedName { get; set; }
    }
}
