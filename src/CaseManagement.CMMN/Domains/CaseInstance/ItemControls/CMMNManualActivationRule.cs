namespace CaseManagement.CMMN.Domains
{
    public class CMMNManualActivationRule : CMMNPlanItemControl
    {
        public CMMNManualActivationRule() { }

        public CMMNManualActivationRule(string name)
        {
            Name = name;
        }

        public CMMNManualActivationRule(string name, CMMNExpression expression) : this(name)
        {
            Expression = expression;
        }

        /// <summary>
        /// The name of the ManualActivationRule.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// An Expression that MUST evaluate to boolean. [1...1]
        /// </summary>
        public CMMNExpression Expression { get; set; }

        public override object Clone()
        {
            return new CMMNManualActivationRule(Name)
            {
                Expression = Expression == null ? null : (CMMNExpression)Expression.Clone()
            };
        }
    }
}
