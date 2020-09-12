using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class EmptyTaskElementInstance : BaseTaskOrStageElementInstance
    {
        public override string Type { get => "emptytask"; }

        public override object Clone()
        {
            var result = new EmptyTaskElementInstance();
            FeedCasePlanElement(result);
            FeedTaskOrStage(result);
            return result;
        }
    }
}
