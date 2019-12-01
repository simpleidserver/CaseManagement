namespace CaseManagement.CMMN.Domains.Events
{
    public class CMMNPlanItemCompleted : CMMNPlanItemTransitionEvent
    {
        public CMMNPlanItemCompleted() : base(CMMNPlanItemTransitions.Complete)
        {
        }
    }
}
