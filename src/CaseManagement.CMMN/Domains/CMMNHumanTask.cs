using CaseManagement.Workflow.Domains;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNHumanTask : CMMNTask
    {
        public CMMNHumanTask(string id, string name) : base(id, name)
        {
        }

        public string FormKey { get; set; }
    }
}
