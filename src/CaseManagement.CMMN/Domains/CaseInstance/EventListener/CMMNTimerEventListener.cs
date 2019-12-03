namespace CaseManagement.CMMN.Domains
{
    public class CMMNTimerEventListener : CMMNEventListener
    {
        public CMMNTimerEventListener(string name) : base(name) { }

        public CMMNEventListenerStates State { get; set; }
        public CMMNExpression TimerExpression { get; set; }

        public override void Handle(CMMNPlanItemTransitions transition)
        {
            switch (transition)
            {
                case CMMNPlanItemTransitions.Create:
                    State = CMMNEventListenerStates.Available;
                    break;
                case CMMNPlanItemTransitions.Occur:
                    State = CMMNEventListenerStates.Completed;
                    break;
            }
        }

        public override object Clone()
        {
            return new CMMNTimerEventListener(Name)
            {
                TimerExpression = TimerExpression == null ? null : (CMMNExpression)TimerExpression.Clone()
            };
        }
    }
}
