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

    public class CMMNSEntry
    {
        public CMMNSEntry(string name)
        {
            Name = name;
            OnParts = new List<CMMNPlanItemOnPart>();
        }

        /// <summary>
        /// Name of the Sentry.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Defines the OnParts of the Sentry.
        /// </summary>
        public ICollection<CMMNPlanItemOnPart> OnParts { get; set; }
        /// <summary>
        /// Defines the IfPart of the Sentry. 
        /// </summary>
        public CMMNIfPart IfPart { get; set; }
    }
}
