using System.Collections.Generic;

namespace CaseManagement.BPMN.Domains
{
    public abstract class BaseActivity : BaseFlowNode
    {
        public BaseActivity()
        {
            StartQuantity = 1;
            CompletionQuantity = 1;
            BoundaryEvtRefs = new List<string>();
        }

        /// <summary>
        ///  This attribute defines the number of tokens that MUST arrive before the activity can begin.
        /// </summary>
        public int StartQuantity { get; set; }
        /// <summary>
        /// This attribute defines the number of tokens that must be generated from the Activity.
        /// </summary>
        public int CompletionQuantity { get; set; }
        public List<string> BoundaryEvtRefs { get; set; }

        protected void FeedActivity(BaseActivity activity)
        {
            FeedFlowNode(activity);
            activity.StartQuantity = StartQuantity;
            activity.CompletionQuantity = CompletionQuantity;
            activity.BoundaryEvtRefs = BoundaryEvtRefs;
        }
    }
}
