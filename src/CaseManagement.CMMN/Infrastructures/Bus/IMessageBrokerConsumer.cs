
namespace CaseManagement.CMMN.Infrastructures.Bus
{
    public interface IMessageBrokerConsumer
    {
        string QueueName { get; }
    }
}
