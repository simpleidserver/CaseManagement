using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.Bus
{
    public class PersistedMessageBroker : IMessageBroker
    {
        private readonly IMessageBrokerStore _messageBrokerStore;

        public PersistedMessageBroker(IMessageBrokerStore messageBrokerStore)
        {
            _messageBrokerStore = messageBrokerStore;
        }

        public async Task<T> Dequeue<T>(string queueName, CancellationToken cancellationToken) where T : class
        {
            var record = await _messageBrokerStore.Dequeue(queueName, cancellationToken);
            if (record == null)
            {
                return null;
            }

            return (T)JsonConvert.DeserializeObject(record.SerializedContent, typeof(T));
        }

        public Task Queue(string queueName, object message, CancellationToken token)
        {
            var record = new QueueMessage
            {
                CreationDateTime = DateTime.UtcNow,
                QueueName = queueName,
                SerializedContent = JsonConvert.SerializeObject(message)
            };
            return _messageBrokerStore.Queue(record, token);
        }

        public Task QueueDeadLetter(string queueName, object msg, DateTime elapsedTime, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<List<DeadLetterMessage>> DequeueDeadLetters(CancellationToken token)
        {
            throw new NotImplementedException();
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
