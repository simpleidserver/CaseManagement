using System.Collections.Generic;

namespace CaseManagement.CMMN.Domains
{
    public class CMMNWorkflowElementDefinition
    {
        public CMMNWorkflowElementDefinition(string id, string name)
        {
            Id = id;
            Name = name;
            EntryCriterions = new List<CMMNCriterion>();
            ExitCriterions = new List<CMMNCriterion>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public CMMNWorkflowElementTypes Type { get; set; }
        /// <summary>
        /// The PlanItemControl controls aspects of the behavior of instances of the PlanItem object. [0...1]
        /// </summary>
        public CMMNActivationRuleTypes? ActivationRule { get; set; }
        public CMMNManualActivationRule ManualActivationRule { get; set; }
        public CMMNRepetitionRule RepetitionRule { get; set; }
        /// <summary>
        /// Zero or more EntryCriterion for that PlanItem. [0...*].
        /// An EntryCriterion represents the condition for a PlanItem to become available.
        /// </summary>
        public ICollection<CMMNCriterion> EntryCriterions { get; set; }
        /// <summary>
        /// An ExitCriterion represents the condition for a PlanItem to terminate. [0...*]
        /// </summary>
        public ICollection<CMMNCriterion> ExitCriterions { get; set; }

        public void SetManualActivationRule(CMMNManualActivationRule activationRule)
        {
            ManualActivationRule = activationRule;
            ActivationRule = CMMNActivationRuleTypes.ManualActivation;
        }

        public void SetRepetitionRule(CMMNRepetitionRule repetitionRule)
        {
            RepetitionRule = repetitionRule;
            ActivationRule = CMMNActivationRuleTypes.Repetition;
        }
    }
}
