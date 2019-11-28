using CaseManagement.Workflow.Domains;
using DynamicExpresso;

namespace CaseManagement.Workflow.Engine
{
    public static class ExpressionParser
    {
        public static bool IsValid(string expressionBody, ProcessFlowInstance flowInstance)
        {
            var interpreter = new Interpreter().SetVariable("context", flowInstance);
            var parsedExpression = interpreter.Parse(expressionBody);
            return (bool)parsedExpression.Invoke();
        }
    }
}
