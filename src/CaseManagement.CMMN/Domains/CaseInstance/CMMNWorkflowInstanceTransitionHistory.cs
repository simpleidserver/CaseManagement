using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNWorkflowInstanceTransitionHistory : ICloneable
    {
        public CMMNWorkflowInstanceTransitionHistory(CMMNTransitions transition, DateTime createDateTime)
        {
            Transition = transition;
            CreateDateTime = createDateTime;
        }

        public CMMNTransitions Transition { get; set; }
        public DateTime CreateDateTime { get; set; }

        public object Clone()
        {
            return new CMMNWorkflowInstanceTransitionHistory(Transition, CreateDateTime);
        }
    }
}
