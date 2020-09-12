using CaseManagement.CMMN.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.Bus
{
    public class InMemoryMessageBroker : IMessageBroker
    {
        public ConcurrentDictionary<string, BlockingCollection<object>> _workingQueues;
        public ConcurrentBag<DeadLetterMessage> _deadLetters;
        

        public InMemoryMessageBroker()
        {
            _workingQueues = new ConcurrentDictionary<string, BlockingCollection<object>>();
            _deadLetters = new ConcurrentBag<DeadLetterMessage>();
        }

        public Task Queue(string queueName, object msg, CancellationToken token)
        {
            if (!_workingQueues.ContainsKey(queueName))
            {
                _workingQueues.TryAdd(queueName, new BlockingCollection<object> { msg });
                return Task.CompletedTask;
            }

            _workingQueues[queueName].Add(msg);
            return Task.CompletedTask;
        }

        public Task QueueDeadLetter(string queueName, object msg, DateTime elapsedTime, CancellationToken token)
        {
            _deadLetters.Add(new DeadLetterMessage
            {
                Content = msg,
                ElapsedTime = elapsedTime,
                QueueName = queueName
            });
            return Task.CompletedTask;
        }

        public Task<T> Dequeue<T>(string queueName, CancellationToken cancellationToken) where T : class
        {
            if (!_workingQueues.ContainsKey(queueName))
            {
                return Task.FromResult((T)null);
            }

            if (_workingQueues[queueName].TryTake(out object msg, 100, cancellationToken))
            {
                return Task.FromResult((T)msg);
            }

            return Task.FromResult((T)null);
        }

        public Task<List<DeadLetterMessage>> DequeueDeadLetters(CancellationToken token)
        {
            var deadLetters = _deadLetters.Where(_ => _.ElapsedTime <= DateTime.UtcNow).ToList();
            for(int i = 0; i < deadLetters.Count; i++)
            {
                var record = deadLetters[i];
                _deadLetters.Remove(record);
            }

            return Task.FromResult(deadLetters);
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
