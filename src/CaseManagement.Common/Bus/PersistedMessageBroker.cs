using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Bus
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
            return Queue(queueName, JsonConvert.SerializeObject(message), token);
        }

        public Task Queue(string queueName, string serializedMessage, CancellationToken token)
        {
            var record = new QueueMessage
            {
                CreationDateTime = DateTime.UtcNow,
                QueueName = queueName,
                SerializedContent = serializedMessage
            };
            return _messageBrokerStore.Queue(record, token);
        }

        public Task QueueScheduledMessage(string queueName, object msg, DateTime elapsedTime, CancellationToken token)
        {
            var record = new ScheduledMessage
            {
                SerializedContent = JsonConvert.SerializeObject(msg),
                ElapsedTime = elapsedTime,
                QueueName = queueName
            };
            return _messageBrokerStore.Queue(record, token);
        }

        public Task<List<ScheduledMessage>> DequeueScheduledMessage(CancellationToken token)
        {
            return _messageBrokerStore.DequeueScheduledMessage(token);
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
