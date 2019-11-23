namespace CaseManagement.CMMN.Domains
{
    public class CMMNManualActivationRule : CMMNPlanItemControl
    {
        public CMMNManualActivationRule(string name)
        {
            Name = name;
        }

        public CMMNManualActivationRule(string name, CMMNExpression expression) : this(name)
        {
            Expression = expression;
        }

        public string Name { get; set; }
        public CMMNExpression Expression { get; set; }
    }
}
