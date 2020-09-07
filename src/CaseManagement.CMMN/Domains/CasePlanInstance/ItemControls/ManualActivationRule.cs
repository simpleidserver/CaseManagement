using CaseManagement.CMMN.Infrastructures;
using System;

namespace CaseManagement.CMMN.Domains
{
    public class ManualActivationRule : ICloneable
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

        public string Name { get; set; }
        public CMMNExpression Expression { get; set; }

        public bool IsSatisfied()
        {
            return ExpressionParser.IsValid(Expression.Body);
        }

        public object Clone()
        {
            return new ManualActivationRule(Name)
            {
                Expression = Expression == null ? null : (CMMNExpression)Expression.Clone()
            };
        }
    }
}
