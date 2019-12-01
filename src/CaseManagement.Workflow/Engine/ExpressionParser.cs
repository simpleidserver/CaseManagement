using CaseManagement.Workflow.Domains;
using DynamicExpresso;
using System.Web;

namespace CaseManagement.Workflow.Engine
{
    public static class ExpressionParser
    {
        public static bool IsValid(string expressionBody, ProcessFlowInstance flowInstance)
        {
            var decodedExpressionBody = HttpUtility.HtmlDecode(expressionBody);
            var interpreter = new Interpreter().SetVariable("context", flowInstance);
            var parsedExpression = interpreter.Parse(decodedExpressionBody);
            return (bool)parsedExpression.Invoke();
        }
    }
}
