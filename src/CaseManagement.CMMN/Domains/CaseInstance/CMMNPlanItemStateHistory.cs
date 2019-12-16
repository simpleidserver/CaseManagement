using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNPlanItemStateHistory : ICloneable
    {
        public CMMNPlanItemStateHistory(DateTime createDateTime, CMMNPlanItemTransitions transition, int version)
        {
            CreateDateTime = createDateTime;
            Transition = transition;
            Version = version;
        }

        public DateTime CreateDateTime { get; set; }
        public CMMNPlanItemTransitions Transition { get; set; }
        public int Version { get; set; }

        public object Clone()
        {
            return new CMMNPlanItemStateHistory(CreateDateTime, Transition, Version);
        }
    }
}
