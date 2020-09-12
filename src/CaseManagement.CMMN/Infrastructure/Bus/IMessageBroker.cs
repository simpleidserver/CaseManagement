using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.Bus
{
    public interface IMessageBroker : IDisposable
    {
        Task Queue(string queueName, object message, CancellationToken token);
        // Task QueueFault(string queueName, object message, CancellationToken token);
        Task<T> Dequeue<T>(string queueName, CancellationToken cancellationToken) where T : class;
        /// <summary>
        /// Enqueue dead letter message.
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="msg"></param>
        /// <param name="elapsedTime"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task QueueDeadLetter(string queueName, object msg, DateTime elapsedTime, CancellationToken token);
        /// <summary>
        /// Dequeue dead letter messages.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<DeadLetterMessage>> DequeueDeadLetters(CancellationToken token);
        Task Start();
        Task Stop();
    }
}