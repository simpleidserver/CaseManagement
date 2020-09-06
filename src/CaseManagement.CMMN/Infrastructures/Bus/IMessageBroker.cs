using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus
{
    public interface IMessageBroker : IDisposable
    {
        Task Queue(string queueName, object message);
        Task<T> Dequeue<T>(string queueName, CancellationToken cancellationToken) where T : class;
        Task Start();
        Task Stop();
    }
}