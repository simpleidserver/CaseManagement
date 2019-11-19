using CaseManagement.Workflow.Domains;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNEndEvent : ProcessFlowInstanceElement
    {
        public BPMNEndEvent(string id, string name) : base(id, name)
        {
        }
    }
}
