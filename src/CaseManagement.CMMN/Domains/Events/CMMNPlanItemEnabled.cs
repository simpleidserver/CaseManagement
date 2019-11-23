namespace CaseManagement.CMMN.Domains.Events
{
    public class CMMNPlanItemEnabled : CMMNPlanItemTransitionEvent
    {
        public CMMNPlanItemEnabled() : base(CMMNPlanItemTransitions.Enable)
        {

        }
    }
}
