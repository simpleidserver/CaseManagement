using CaseManagement.CMMN.Domains;
using DynamicExpresso;
using System;
using System.Web;

namespace CaseManagement.CMMN.Infrastructures
{
    public static class ExpressionParser
    {
        public static bool IsValid(string expressionBody, Domains.CaseInstance flowInstance)
        {
            var decodedExpressionBody = HttpUtility.HtmlDecode(expressionBody);
            var interpreter = new Interpreter().SetVariable("context", flowInstance);
            var parsedExpression = interpreter.Parse(decodedExpressionBody);
            return (bool)parsedExpression.Invoke();
        }

        public static string GetStringEvaluation(string expressionBody, Domains.CaseInstance flowInstance)
        {
            var decodedExpressionBody = HttpUtility.HtmlDecode(expressionBody);
            var interpreter = new Interpreter().SetVariable("context", flowInstance);
            var parsedExpression = interpreter.Parse(decodedExpressionBody);
            return (string)parsedExpression.Invoke();
        }

        public static string GetStringEvaluation(string expressionBody, Domains.CaseInstance flowInstance, Action<Interpreter> callback)
        {
            var decodedExpressionBody = HttpUtility.HtmlDecode(expressionBody);
            var interpreter = new Interpreter();
            interpreter.SetVariable("context", flowInstance);
            callback(interpreter);
            var parsedExpression = interpreter.Parse(decodedExpressionBody);
            return (string)parsedExpression.Invoke();
        }
    }
}
