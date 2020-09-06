using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus
{
    public class InMemoryMessageBroker : IMessageBroker
    {
        public ConcurrentDictionary<string, BlockingCollection<object>> _queues;

        public InMemoryMessageBroker()
        {
            _queues = new ConcurrentDictionary<string, BlockingCollection<object>>();
        }

        public Task Queue(string queueName, object msg)
        {
            if (!_queues.ContainsKey(queueName))
            {
                _queues.TryAdd(queueName, new BlockingCollection<object> { msg });
                return Task.CompletedTask;
            }

            _queues[queueName].Add(msg);
            return Task.CompletedTask;
        }

        public Task<T> Dequeue<T>(string queueName, CancellationToken cancellationToken) where T : class
        {
            if (!_queues.ContainsKey(queueName))
            {
                return Task.FromResult((T)null);
            }

            if (_queues[queueName].TryTake(out object msg, 100, cancellationToken))
            {
                return Task.FromResult((T)msg);
            }

            return Task.FromResult((T)null);
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
