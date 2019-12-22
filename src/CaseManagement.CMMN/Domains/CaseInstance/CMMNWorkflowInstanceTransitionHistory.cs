using System;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNWorkflowInstanceTransitionHistory
    {
        public CMMNWorkflowInstanceTransitionHistory(CMMNTransitions transition, DateTime createDateTime)
        {
            Transition = transition;
            CreateDateTime = createDateTime;
        }

        public CMMNTransitions Transition { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
