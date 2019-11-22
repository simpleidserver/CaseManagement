using CaseManagement.Workflow.Domains;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNPlanItemOnPart
    {
        /// <summary>
        /// Reference to a state transition in the lifecycle of a Stage, Task, EventListener or Milestone.
        /// </summary>
        public CMMNPlanItemTransitions StandardEvent { get; set; }
        /// <summary>
        /// Refer to a PlanItem.
        /// </summary>
        public string SourceRef { get; set; }
        // Exit criterion ?
    }

    public class CMMNIfPart
    {
        // contextRef ?
        /// <summary>
        /// Condition that is defined as Expression.
        /// </summary>
        public string Condition { get; set; }
    }

    public class CMMNSEntry : ProcessFlowInstanceElement
    {
        public CMMNSEntry(string id, string name) : base(id, name)
        {
            OnParts = new List<CMMNPlanItemOnPart>();
        }

        public ICollection<CMMNPlanItemOnPart> OnParts { get; set; }
        /// <summary>
        /// Specify an (optional) condition.
        /// </summary>
        public CMMNIfPart IfPart { get; set; }
    }
}
