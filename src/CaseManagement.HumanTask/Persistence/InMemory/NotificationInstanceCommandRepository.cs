using CaseManagement.HumanTask.Domains;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.InMemory
{
    public class NotificationInstanceCommandRepository : INotificationInstanceCommandRepository
    {
        private readonly ConcurrentBag<NotificationInstanceAggregate> _notifications;

        public NotificationInstanceCommandRepository(ConcurrentBag<NotificationInstanceAggregate> notifications)
        {
            _notifications = notifications;
        }

        public Task<bool> Add(NotificationInstanceAggregate notification, CancellationToken token)
        {
            _notifications.Add((NotificationInstanceAggregate)notification.Clone());
            return Task.FromResult(true);
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}
