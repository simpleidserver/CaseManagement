using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNWorkflowElementInstanceTransitionHistory : ICloneable
    {
        public CMMNWorkflowElementInstanceTransitionHistory(CMMNTransitions transition, DateTime createDateTime)
        {
            Transition = transition;
            CreateDateTime = createDateTime;
        }

        public CMMNTransitions Transition { get; set; }
        public DateTime CreateDateTime { get; set; }

        public object Clone()
        {
            return new CMMNWorkflowElementInstanceTransitionHistory(Transition, CreateDateTime);
        }
    }
}
