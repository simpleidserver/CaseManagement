namespace CaseManagement.MessageBroker
{
    public interface IMessageBrokerConsumer
    {
        string QueueName { get; }
    }
}
