using System;

namespace CaseManagement.CMMN.Domains
{
    public class CasePlanElementInstanceTransitionHistory
    {
        public DateTime ExecutionDateTime { get; set; }
        public CMMNTransitions Transition { get; set; }
    }
}
