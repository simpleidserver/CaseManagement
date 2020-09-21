using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Bus
{
    public interface IMessageBroker : IDisposable
    {
        Task Queue(string queueName, object message, CancellationToken token);
        Task Queue(string queueName, string serializedMessage, CancellationToken token);
        // Task QueueFault(string queueName, object message, CancellationToken token);
        Task<T> Dequeue<T>(string queueName, CancellationToken cancellationToken) where T : class;
        /// <summary>
        /// Enqueue schedule message.
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="msg"></param>
        /// <param name="elapsedTime"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task QueueScheduledMessage(string queueName, object msg, DateTime elapsedTime, CancellationToken token);
        /// <summary>
        /// Dequeue scheduled messages.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<ScheduledMessage>> DequeueScheduledMessage(CancellationToken token);
        Task Start();
        Task Stop();
    }
}