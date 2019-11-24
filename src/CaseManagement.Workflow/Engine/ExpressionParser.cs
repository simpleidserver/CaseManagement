using DynamicExpresso;

namespace CaseManagement.Workflow.Engine
{
    public static class ExpressionParser
    {
        public static bool IsValid(string expressionBody, ProcessFlowInstanceExecutionContext context)
        {
            var interpreter = new Interpreter().SetVariable("context", context);
            var parsedExpression = interpreter.Parse(expressionBody);
            return (bool)parsedExpression.Invoke();
        }
    }
}
