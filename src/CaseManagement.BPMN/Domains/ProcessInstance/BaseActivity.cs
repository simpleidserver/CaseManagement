namespace CaseManagement.BPMN.Domains
{
    public abstract class BaseActivity : BaseFlowNode
    {
        public BaseActivity()
        {
            StartQuantity = 1;
            CompletionQuantity = 1;
        }

        public ActivityStates? State { get; set; }
        /// <summary>
        ///  This attribute defines the number of tokens that MUST arrive before the activity can begin.
        /// </summary>
        public int StartQuantity { get; set; }
        /// <summary>
        /// This attribute defines the number of tokens that must be generated from the Activity.
        /// </summary>
        public int CompletionQuantity { get; set; }

        protected void FeedActivity(BaseActivity activity)
        {
            FeedFlowNode(activity);
            activity.State = State;
            activity.StartQuantity = StartQuantity;
            activity.CompletionQuantity = CompletionQuantity;
        }

        protected override void UpdateState(BPMNTransitions transition)
        {
            switch(transition)
            {
                case BPMNTransitions.ACTIVITYREADY:
                    State = ActivityStates.READY;
                    break;
                case BPMNTransitions.ACTIVITYACTIVE:
                    State = ActivityStates.ACTIVE;
                    break;
                case BPMNTransitions.ACTIVITYCOMPLETE:
                    State = ActivityStates.COMPLETING;
                    break;
                case BPMNTransitions.COMPLETE:
                    State = ActivityStates.COMPLETED;
                    break;
            }
        }
    }
}
