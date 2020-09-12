﻿using System;
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

    [Serializable]
    public class PlanItemOnPart : IOnPart
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

        public object Clone()
        {
            return new PlanItemOnPart
            {
                SourceRef = SourceRef,
                StandardEvent = StandardEvent
            };
        }
    }

    [Serializable]
    public class CaseFileItemOnPart : IOnPart
    {
        /// <summary>
        /// Refer to a CaseFileItem.
        /// </summary>
        public string SourceRef { get; set; }

        /// <summary>
        ///  Reference to a state transition in the CaseFileItem lifecyle.
        /// </summary>
        public CMMNTransitions StandardEvent { get; set; }

        public OnPartTypes Type => OnPartTypes.FileItem;

        public object Clone()
        {
            return new CaseFileItemOnPart
            {
                StandardEvent = StandardEvent,
                SourceRef = SourceRef
            };
        }
    }

    [Serializable]
    public class IfPart : ICloneable
    {
        // contextRef ?
        /// <summary>
        /// Condition that is defined as Expression.
        /// </summary>
        public string Condition { get; set; }

        public object Clone()
        {
            return new IfPart
            {
                Condition = Condition
            };
        }
    }

    [Serializable]
    public class SEntry : ICloneable
    {
        public SEntry(string name)
        {
            Name = name;
            PlanItemOnParts = new List<PlanItemOnPart>();
            FileItemOnParts = new List<CaseFileItemOnPart>();
        }

        /// <summary>
        /// Name of the Sentry.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Defines the OnParts of the Sentry.
        /// </summary>
        public ICollection<PlanItemOnPart> PlanItemOnParts { get; set; }
        /// <summary>
        /// Defines the OnParts of the Sentry.
        /// </summary>
        public ICollection<CaseFileItemOnPart> FileItemOnParts { get; set; }
        /// <summary>
        /// Defines the IfPart of the Sentry. 
        /// </summary>
        public IfPart IfPart { get; set; }

        public object Clone()
        {
            return new SEntry(Name)
            {
                IfPart = IfPart == null ? null : (IfPart)IfPart.Clone(),
                PlanItemOnParts = PlanItemOnParts.Select(p => (PlanItemOnPart)p.Clone()).ToList(),
                FileItemOnParts = FileItemOnParts.Select(p => (CaseFileItemOnPart)p.Clone()).ToList()
            };
        }
    }
}
