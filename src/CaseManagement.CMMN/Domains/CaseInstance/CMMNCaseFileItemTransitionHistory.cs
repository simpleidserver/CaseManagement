using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNCaseFileItemTransitionHistory : ICloneable
    {
        public CMMNCaseFileItemTransitionHistory(DateTime createDateTime, CMMNCaseFileItemTransitions transition)
        {
            CreateDateTime = createDateTime;
            Transition = transition;
        }

        public DateTime CreateDateTime { get; set; }
        public CMMNCaseFileItemTransitions Transition { get; set; }

        public object Clone()
        {
            return new CMMNCaseFileItemTransitionHistory(CreateDateTime, Transition);
        }
    }
}
