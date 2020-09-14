using System;
using System.Collections.Concurrent;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class EmptyTaskElementInstance : BaseTaskOrStageElementInstance
    {
        public override CasePlanElementInstanceTypes Type { get => CasePlanElementInstanceTypes.EMPTYTASK; }

        public override object Clone()
        {
            var result = new EmptyTaskElementInstance();
            FeedCaseEltInstance(result);
            FeedCasePlanItem(result);
            FeedTaskOrStage(result);
            return result;
        }

        public override BaseCasePlanItemInstance NewOccurrence(string casePlanInstanceId)
        {
            var clone = Clone() as EmptyTaskElementInstance;
            clone.State = null;
            clone.TransitionHistories = new ConcurrentBag<CasePlanElementInstanceTransitionHistory>();
            clone.NbOccurrence = NbOccurrence + 1;
            clone.Id = BuildId(casePlanInstanceId, EltId, clone.NbOccurrence);
            return clone;
        }
    }
}
