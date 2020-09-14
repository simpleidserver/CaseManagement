using System;
using System.Collections.Concurrent;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class MilestoneElementInstance : BaseMilestoneOrTimerElementInstance
    {
        public override CasePlanElementInstanceTypes Type { get => CasePlanElementInstanceTypes.MILESTONE; }

        public override object Clone()
        {
            var result = new MilestoneElementInstance
            {
                State = State
            };
            FeedCaseEltInstance(result);
            FeedCasePlanItem(result);
            FeedMilestoneOrTimer(result);
            return result;
        }

        public override BaseCasePlanItemInstance NewOccurrence(string casePlanInstanceId)
        {
            var clone = Clone() as MilestoneElementInstance;
            clone.State = null;
            clone.TransitionHistories = new ConcurrentBag<CasePlanElementInstanceTransitionHistory>();
            clone.NbOccurrence = NbOccurrence + 1;
            clone.Id = BuildId(casePlanInstanceId, EltId, clone.NbOccurrence);
            return clone;
        }
    }
}
