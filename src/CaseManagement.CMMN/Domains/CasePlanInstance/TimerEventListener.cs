using System;
using System.Collections.Concurrent;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class TimerEventListener : BaseMilestoneOrTimerElementInstance
    {
        public CMMNExpression TimerExpression { get; set; }
        public override CasePlanElementInstanceTypes Type { get => CasePlanElementInstanceTypes.TIMER; }

        public override object Clone()
        {
            var result = new TimerEventListener
            {
                TimerExpression = TimerExpression == null ? null : (CMMNExpression)TimerExpression.Clone()
            };
            FeedCaseEltInstance(result);
            FeedCasePlanItem(result);
            FeedMilestoneOrTimer(result);
            return result;
        }

        public override BaseCasePlanItemInstance NewOccurrence(string casePlanInstanceId)
        {
            var clone = Clone() as TimerEventListener;
            clone.State = null;
            clone.TransitionHistories = new ConcurrentBag<CasePlanElementInstanceTransitionHistory>();
            clone.NbOccurrence = NbOccurrence + 1;
            clone.Id = BuildId(casePlanInstanceId, EltId, clone.NbOccurrence);
            return clone;
        }
    }
}
