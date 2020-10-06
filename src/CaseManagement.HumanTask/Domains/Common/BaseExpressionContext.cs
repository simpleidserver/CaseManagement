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

        /// <summary>
        /// Get the input parameter of the current task / notification.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetInput(string key)
        {
            return GetParameter(_operationParameters, key);
        }

        protected string GetParameter(Dictionary<string, string> parameters, string key)
        {
            if (!parameters.ContainsKey(key))
            {
                return string.Empty;
            }

            return parameters[key];
        }
    }
}
