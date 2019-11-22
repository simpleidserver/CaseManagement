using CaseManagement.Workflow.Domains;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNPlanItem : ProcessFlowInstanceElement
    {
        public CMMNPlanItem(string id, string name) : base(id, name)
        {
            Transition = CMMNPlanItemTransitions.Enable;
            SEntries = new List<CMMNSEntry>();
        }

        public CMMNPlanItemTransitions Transition { get; set; }
        public ICollection<CMMNSEntry> SEntries { get; set; }
    }
}
