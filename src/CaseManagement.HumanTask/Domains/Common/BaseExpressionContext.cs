using CaseManagement.Common.Expression;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Domains
{
    public class BaseExpressionContext : ExpressionExecutionContext
    {
        private readonly Dictionary<string, string> _operationParameters;

        public BaseExpressionContext(Dictionary<string, string> operationParameters)
        {
            _operationParameters = operationParameters;
        }

        public string GetInput(string key)
        {
            if (!_operationParameters.ContainsKey(key))
            {
                return string.Empty;
            }

            return _operationParameters[key];
        }
    }
}
