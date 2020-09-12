using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class MilestoneElementInstance : BaseMilestoneOrTimerElementInstance
    {
        public override string Type { get => "milestone"; }

        public override object Clone()
        {
            var result = new MilestoneElementInstance
            {
                State = State
            };
            FeedCasePlanElement(result);
            FeedMilestoneOrTimer(result);
            return result;
        }
    }
}
