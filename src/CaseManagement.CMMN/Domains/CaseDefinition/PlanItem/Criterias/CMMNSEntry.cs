using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public enum OnPartTypes
    {
        PlanItem = 0,
        FileItem = 1
    }

    public interface IOnPart : ICloneable
    {
        OnPartTypes Type { get; }
    }

    public class CMMNPlanItemOnPart : IOnPart
    {
        /// <summary>
        /// Refer to a PlanItem.
        /// </summary>
        public string SourceRef { get; set; }

        /// <summary>
        /// Reference to a state transition in the lifecycle of a Stage, Task, EventListener or Milestone.
        /// </summary>
        public CMMNTransitions StandardEvent { get; set; }

        public OnPartTypes Type => OnPartTypes.PlanItem;

        public object  Clone()
        {
            return new CMMNPlanItemOnPart
            {
                SourceRef = SourceRef,
                StandardEvent = StandardEvent
            };
        }
    }

    public class CMMNCaseFileItemOnPart : IOnPart
    {
        /// <summary>
        /// Refer to a CaseFileItem.
        /// </summary>
        public string SourceRef { get; set; }

        /// <summary>
        ///  Reference to a state transition in the CaseFileItem lifecyle.
        /// </summary>
        public CMMNCaseFileItemTransitions StandardEvent { get; set; }

        public OnPartTypes Type => OnPartTypes.FileItem;

        public object Clone()
        {
            return new CMMNCaseFileItemOnPart
            {
                StandardEvent = StandardEvent,
                SourceRef = SourceRef
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
            PlanItemOnParts = new List<CMMNPlanItemOnPart>();
            FileItemOnParts = new List<CMMNCaseFileItemOnPart>();
        }

        /// <summary>
        /// Name of the Sentry.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Defines the OnParts of the Sentry.
        /// </summary>
        public ICollection<CMMNPlanItemOnPart> PlanItemOnParts { get; set; }
        /// <summary>
        /// Defines the OnParts of the Sentry.
        /// </summary>
        public ICollection<CMMNCaseFileItemOnPart> FileItemOnParts { get; set; }
        /// <summary>
        /// Defines the IfPart of the Sentry. 
        /// </summary>
        public CMMNIfPart IfPart { get; set; }

        public object Clone()
        {
            return new CMMNSEntry(Name)
            {
                IfPart = IfPart == null ? null : (CMMNIfPart)IfPart.Clone(),
                PlanItemOnParts = PlanItemOnParts.Select(p => (CMMNPlanItemOnPart)p.Clone()).ToList(),
                FileItemOnParts = FileItemOnParts.Select(p => (CMMNCaseFileItemOnPart)p.Clone()).ToList()
            };
        }
    }
}
