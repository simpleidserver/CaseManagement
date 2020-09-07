using DynamicExpresso;
using System.Web;

namespace CaseManagement.CMMN.Infrastructures
{
    public static class ExpressionParser
    {
        public static bool IsValid(string expressionBody)
        {
            var decodedExpressionBody = HttpUtility.HtmlDecode(expressionBody);
            var interpreter = new Interpreter();
            var parsedExpression = interpreter.Parse(decodedExpressionBody);
            return (bool)parsedExpression.Invoke();
        }
    }
}
