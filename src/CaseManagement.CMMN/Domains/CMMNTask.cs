using CaseManagement.Workflow.Domains;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNTask : ProcessFlowInstanceElement
    {
        public CMMNTask(string id, string name) : base(id, name)
        {
        }

        public bool IsBlocking { get; set; }
    }
}
