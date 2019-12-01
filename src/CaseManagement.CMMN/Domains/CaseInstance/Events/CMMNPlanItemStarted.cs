namespace CaseManagement.CMMN.Domains.Events
{
    public class CMMNPlanItemStarted : CMMNPlanItemTransitionEvent
    {
        public CMMNPlanItemStarted() : base(CMMNPlanItemTransitions.Start)
        {
        }
    }
}
