using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus
{
    public interface IMessageBrokerConsumerGeneric<T> : IMessageBrokerConsumer where T : class
    {
        Task Handle(T message, CancellationToken token);
    }
}
