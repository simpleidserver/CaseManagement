using System;

namespace CaseManagement.CMMN.Domains
{
    public class CaseInstanceTransitionHistory : ICloneable
    {
        public CaseInstanceTransitionHistory(CMMNTransitions transition, DateTime createDateTime)
        {
            Transition = transition;
            CreateDateTime = createDateTime;
        }

        public CMMNTransitions Transition { get; set; }
        public DateTime CreateDateTime { get; set; }

        public object Clone()
        {
            return new CaseInstanceTransitionHistory(Transition, CreateDateTime);
        }
    }
}
