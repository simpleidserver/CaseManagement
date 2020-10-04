using DynamicExpresso;
using System;
using System.Web;

namespace CaseManagement.Common.Expression
{
    public class ExpressionParser
    {
        public static bool IsValid<T>(string expressionBody, T executionContext) where T : ExpressionExecutionContext
        {
            return (bool)GetObj<T>(expressionBody, executionContext);
        }

        public static string GetString<T>(string expressionBody, T executionContext) where T : ExpressionExecutionContext
        {
            return GetObj<T>(expressionBody, executionContext).ToString();
        }

        public static DateTime? GetDateTime<T>(string expressionBody, T executionContext) where T : ExpressionExecutionContext
        {
            var result = GetObj<T>(expressionBody, executionContext);
            if (result == null)
            {
                return null;
            }

            if (DateTime.TryParse(result.ToString(), out DateTime res))
            {
                return res;
            }

            return null;
        }

        private static object GetObj<T>(string expressionBody, T executionContext) where T : ExpressionExecutionContext
        {
            var decodedExpressionBody = HttpUtility.HtmlDecode(expressionBody);
            var interpreter = new Interpreter().SetVariable("context", executionContext);
            var parsedExpression = interpreter.Parse(decodedExpressionBody);
            return parsedExpression.Invoke();
        }
    }
}
