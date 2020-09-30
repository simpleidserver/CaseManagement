using CaseManagement.Common.Expression;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskInstanceExpressionContext : ExpressionExecutionContext
    {
        private readonly HumanTaskInstanceAggregate _humanTaskInstance;

        public HumanTaskInstanceExpressionContext(HumanTaskInstanceAggregate humanTaskInstance)
        {
            _humanTaskInstance = humanTaskInstance;
        }

        public string GetInput(string key)
        {
            if (!_humanTaskInstance.OperationParameters.ContainsKey(key))
            {
                return string.Empty;
            }

            return _humanTaskInstance.OperationParameters[key];
        }
    }
}
