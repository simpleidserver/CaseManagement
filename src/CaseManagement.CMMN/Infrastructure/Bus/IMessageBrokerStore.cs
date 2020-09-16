using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.Bus
{
    public interface IMessageBrokerStore
    {
        Task Queue(QueueMessage message, CancellationToken token);
        Task Queue(ScheduledMessage message, CancellationToken token);
        Task<QueueMessage> Dequeue(string queueName, CancellationToken cancellationToken);
        Task<List<ScheduledMessage>> DequeueScheduledMessage(CancellationToken token);
    }
}
