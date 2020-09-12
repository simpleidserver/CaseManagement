using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class TimerEventListener : BaseMilestoneOrTimerElementInstance
    {
        public CMMNExpression TimerExpression { get; set; }
        public override string Type { get => "timer"; }

        public override object Clone()
        {
            var result = new TimerEventListener
            {
                TimerExpression = TimerExpression == null ? null : (CMMNExpression)TimerExpression.Clone()
            };
            FeedCasePlanElement(result);
            FeedMilestoneOrTimer(result);
            return result;
        }
    }
}
