using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CasePlanElement : ICloneable
    {
        public CasePlanElement(string id, string name)
        {
            Id = id;
            Name = name;
            EntryCriterions = new List<Criteria>();
            ExitCriterions = new List<Criteria>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public CaseElementTypes Type { get; set; }
        /// <summary>
        /// The PlanItemControl controls aspects of the behavior of instances of the PlanItem object. [0...1]
        /// </summary>
        public ActivationRuleTypes? ActivationRule { get; set; }
        public ManualActivationRule ManualActivationRule { get; set; }
        public RepetitionRule RepetitionRule { get; set; }
        /// <summary>
        /// Zero or more EntryCriterion for that PlanItem. [0...*].
        /// An EntryCriterion represents the condition for a PlanItem to become available.
        /// </summary>
        public ICollection<Criteria> EntryCriterions { get; set; }
        /// <summary>
        /// An ExitCriterion represents the condition for a PlanItem to terminate. [0...*]
        /// </summary>
        public ICollection<Criteria> ExitCriterions { get; set; }
        /// <summary>
        /// Get or set the table item.
        /// </summary>
        public TableItem TableItem { get; set; }

        public virtual object Clone()
        {
            return new CasePlanElement(Id, Name)
            {
                Type = Type,
                ActivationRule = ActivationRule,
                ManualActivationRule = ManualActivationRule == null ? null : (ManualActivationRule)ManualActivationRule.Clone(),
                RepetitionRule = RepetitionRule == null ? null : (RepetitionRule)RepetitionRule.Clone(),
                EntryCriterions = EntryCriterions.Select(e => (Criteria)e.Clone()).ToList(),
                ExitCriterions = ExitCriterions.Select(e => (Criteria)e.Clone()).ToList(),
                TableItem = TableItem == null ? null : (TableItem)TableItem.Clone()
            };
        }

        public bool IsDiscrete()
        {
            return TableItem != null;
        }

        public void SetManualActivationRule(ManualActivationRule activationRule)
        {
            ManualActivationRule = activationRule;
            ActivationRule = ActivationRuleTypes.ManualActivation;
        }

        public void SetRepetitionRule(RepetitionRule repetitionRule)
        {
            RepetitionRule = repetitionRule;
            ActivationRule = ActivationRuleTypes.Repetition;
        }
    }
}
