using System;
using System.Threading.Tasks;

namespace CaseManagement.MessageBroker
{
    public interface IMessageBroker : IDisposable
    {
        Task Start();
        Task Queue<T>(string queueName, T message);
        Task Stop();
    }
}