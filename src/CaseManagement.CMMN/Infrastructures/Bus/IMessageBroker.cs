using System;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus
{
    public interface IMessageBroker : IDisposable, IJob
    {
        Task Queue(string queueName, object message);
    }
}