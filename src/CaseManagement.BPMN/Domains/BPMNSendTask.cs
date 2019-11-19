using CaseManagement.Workflow.Domains;

namespace CaseManagement.BPMN.Domains
{
    public class BPMNSendTask : ProcessFlowInstanceElement
    {
        public BPMNSendTask(string id, string name):  base(id, name) { }
    }
}
