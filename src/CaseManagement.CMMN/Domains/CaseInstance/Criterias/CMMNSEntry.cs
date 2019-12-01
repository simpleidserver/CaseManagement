using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNPlanItemOnPart : ICloneable
    {
        /// <summary>
        /// Reference to a state transition in the lifecycle of a Stage, Task, EventListener or Milestone.
        /// </summary>
        public CMMNPlanItemTransitions StandardEvent { get; set; }
        /// <summary>
        /// Refer to a PlanItem.
        /// </summary>
        public string SourceRef { get; set; }

        public object Clone()
        {
            return new CMMNPlanItemOnPart
            {
                SourceRef = SourceRef,
                StandardEvent = StandardEvent
            };
        }
    }

    public class CMMNIfPart : ICloneable
    {
        // contextRef ?
        /// <summary>
        /// Condition that is defined as Expression.
        /// </summary>
        public string Condition { get; set; }

        public object Clone()
        {
            return new CMMNIfPart
            {
                Condition = Condition
            };
        }
    }

    public class CMMNSEntry : ICloneable
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

        public object Clone()
        {
            return new CMMNSEntry(Name)
            {
                IfPart = IfPart == null ? null : (CMMNIfPart)IfPart.Clone(),
                OnParts = OnParts.Select(p => (CMMNPlanItemOnPart)p.Clone()).ToList()
            };
        }
    }
}
