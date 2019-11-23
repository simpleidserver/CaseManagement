namespace CaseManagement.CMMN.Domains.Events
{
    public class CMMNPlanItemCreated : CMMNPlanItemTransitionEvent
    {
        public CMMNPlanItemCreated() : base(CMMNPlanItemTransitions.Create)
        {

        }
    }
}