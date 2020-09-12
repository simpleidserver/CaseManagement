using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class HumanTaskElementInstance : BaseTaskOrStageElementInstance
    {
        /// <summary>
        /// The performer of the humanTask (role [0...1]).
        /// </summary>
        public string PerformerRef { get; set; }
        public string FormId { get; set; }
        public override string Type { get => "humantask"; }

        public override object Clone()
        {
            var result = new HumanTaskElementInstance
            {
                PerformerRef = PerformerRef,
                FormId = FormId
            };
            FeedCasePlanElement(result);
            FeedTaskOrStage(result);
            return result;
        }
    }
}
