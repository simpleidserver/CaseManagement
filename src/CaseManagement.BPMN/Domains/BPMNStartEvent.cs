using CaseManagement.Workflow.Domains;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNStartEvent : ProcessFlowInstanceElement
    {
        public BPMNStartEvent(string id, string name) : base(id, name)
        {
        }
    }
}
