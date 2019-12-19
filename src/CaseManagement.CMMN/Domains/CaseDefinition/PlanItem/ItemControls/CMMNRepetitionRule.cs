namespace CaseManagement.CMMN.Domains
{
    public class CMMNRepetitionRule : CMMNPlanItemControl
    {
        public CMMNRepetitionRule() { }

        public CMMNRepetitionRule(string name)
        {
            Name = name;
        }

        public CMMNRepetitionRule(string name, CMMNExpression condition) : this(name)
        {
            Condition = condition;
        }

        /// <summary>
        /// The name of the RepetitionRule.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// An Expression that MUST evaluate to boolean. If the Expression evaluates to TRUE, then the instance of the Task, Stage, or Milestone may be repeated, otherwise it MUST NOT be repeated.
        /// </summary>
        public CMMNExpression Condition { get; set; }

        public override object Clone()
        {
            return new CMMNRepetitionRule(Name)
            {
                Condition = Condition == null ? null : (CMMNExpression)Condition.Clone()
            };
        }
    }
}
