namespace CaseManagement.CMMN.Domains.Events
{
    public class CMMNPlanItemManuallyStarted : CMMNPlanItemTransitionEvent
    {
        public CMMNPlanItemManuallyStarted() : base(CMMNPlanItemTransitions.ManualStart)
        {
        }
    }
}
