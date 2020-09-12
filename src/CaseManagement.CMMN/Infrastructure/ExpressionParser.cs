using DynamicExpresso;
using CaseManagement.CMMN.Domains;
using System.Web;

namespace CaseManagement.CMMN.Infrastructure
{
    public class ExpressionParser
    {
        public static bool IsValid(string expressionBody, CasePlanInstanceExecutionContext executionContext)
        {
            var decodedExpressionBody = HttpUtility.HtmlDecode(expressionBody);
            var interpreter = new Interpreter().SetVariable("context", executionContext);
            var parsedExpression = interpreter.Parse(decodedExpressionBody);
            return (bool)parsedExpression.Invoke();
        }
    }
}