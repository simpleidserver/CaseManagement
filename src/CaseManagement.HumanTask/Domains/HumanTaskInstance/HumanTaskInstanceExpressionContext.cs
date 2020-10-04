namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskInstanceExpressionContext : BaseExpressionContext
    {
        public HumanTaskInstanceExpressionContext(HumanTaskInstanceAggregate humanTaskInstance) : base(humanTaskInstance.OperationParameters)
        {
        }
    }
}
