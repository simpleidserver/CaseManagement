using CaseManagement.CMMN.Domains;
using System.Linq;

namespace CaseManagement.Workflow.Domains
{
    public static class ProcessFlowInstanceExtensions
    {
        public static bool DisablePlanItem(this ProcessFlowInstance processFlowInstance, string id)
        {
            var elt = processFlowInstance.Elements.FirstOrDefault(e => e.Id == id) as CMMNPlanItem;
            if (elt == null)
            {
                return false;
            }

            elt.Disable();
            return true;
        }

        public static bool ManuallyStartPlanItem(this ProcessFlowInstance processFlowInstance, string id)
        {
            var elt = processFlowInstance.Elements.FirstOrDefault(e => e.Id == id) as CMMNPlanItem;
            if (elt == null)
            {
                return false;
            }

            elt.ManualStart();
            return true;
        }
    }
}
