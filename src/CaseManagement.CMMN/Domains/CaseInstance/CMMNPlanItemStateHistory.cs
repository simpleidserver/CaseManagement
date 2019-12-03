using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNPlanItemStateHistory : ICloneable
    {
        public CMMNPlanItemStateHistory(DateTime createDateTime, CMMNPlanItemTransitions transition)
        {
            CreateDateTime = createDateTime;
            Transition = transition;
        }

        public DateTime CreateDateTime { get; set; }
        public CMMNPlanItemTransitions Transition { get; set; }

        public object Clone()
        {
            return new CMMNPlanItemStateHistory(CreateDateTime, Transition);
        }
    }
}
