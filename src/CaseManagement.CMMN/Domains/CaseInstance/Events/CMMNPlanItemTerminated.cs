namespace CaseManagement.CMMN.Domains.Events
{
    public class CMMNPlanItemTerminated : CMMNPlanItemTransitionEvent
    {
        public CMMNPlanItemTerminated() : base(CMMNPlanItemTransitions.Terminate)
        {
        }
    }
}
