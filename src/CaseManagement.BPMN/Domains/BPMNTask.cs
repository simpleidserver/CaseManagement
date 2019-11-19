using CaseManagement.Workflow.Domains;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNTask : ProcessFlowInstanceElement
    {
        public BPMNTask(string id, string name) : base(id, name)
        {
        }
    }
}