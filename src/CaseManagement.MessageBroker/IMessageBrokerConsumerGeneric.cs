using System.Threading.Tasks;

namespace CaseManagement.MessageBroker
{
    public interface IMessageBrokerConsumerGeneric<T> : IMessageBrokerConsumer where T : class
    {
        Task Handle(T message);
    }
}
