namespace CaseManagement.HumanTask.Domains
{
    public class NotificationInstanceExpressionContext : BaseExpressionContext
    {
        public NotificationInstanceExpressionContext(NotificationInstanceAggregate notificationContext) : base(notificationContext.OperationParameters)
        {
        }

        public string GetPotentialOwner()
        {
            return null;
        }
    }
}
