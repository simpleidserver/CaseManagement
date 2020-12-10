using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class HumanTaskElementInstance : BaseTaskOrStageElementInstance
    {
        public HumanTaskElementInstance()
        {
            InputParameters = new Dictionary<string, string>();
        }

        #region Properties

        /// <summary>
        /// The performer of the humanTask (role [0...1]).
        /// </summary>
        public string PerformerRef { get; set; }
        public string Implemention { get; set; }
        public string FormId { get; set; }
        public Dictionary<string, string> InputParameters { get; set; }
        public override CasePlanElementInstanceTypes Type { get => CasePlanElementInstanceTypes.HUMANTASK; }

        #endregion

        public override object Clone()
        {
            var result = new HumanTaskElementInstance
            {
                PerformerRef = PerformerRef,
                FormId = FormId,
                Implemention = Implemention,
                InputParameters = InputParameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            };
            FeedCaseEltInstance(result);
            FeedCasePlanItem(result);
            FeedTaskOrStage(result);
            return result;
        }

        public override BaseCasePlanItemInstance NewOccurrence(string casePlanInstanceId)
        {
            var clone = Clone() as HumanTaskElementInstance;
            clone.State = null;
            clone.TransitionHistories = new ConcurrentBag<CasePlanElementInstanceTransitionHistory>();
            clone.NbOccurrence = NbOccurrence + 1;
            clone.Id = BuildId(casePlanInstanceId, EltId, clone.NbOccurrence);
            return clone;
        }
    }
}
