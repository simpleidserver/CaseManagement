namespace CaseManagement.HumanTask.Domains
{
    public class ExpressionAssignmentDefinition : PeopleAssignmentDefinition
    {
        public override PeopleAssignmentTypes Type => PeopleAssignmentTypes.EXPRESSION;

        public string Expression { get; set; }

        public override object Clone()
        {
            return new ExpressionAssignmentDefinition
            {
                Expression = Expression
            };
        }
    }
}
