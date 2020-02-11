namespace CaseManagement.CMMN.Domains
{
    public class ManualActivationRule : PlanItemControl
    {
        public ManualActivationRule() { }

        public ManualActivationRule(string name)
        {
            Name = name;
        }

        public ManualActivationRule(string name, CMMNExpression expression) : this(name)
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
            return new ManualActivationRule(Name)
            {
                Expression = Expression == null ? null : (CMMNExpression)Expression.Clone()
            };
        }
    }
}
