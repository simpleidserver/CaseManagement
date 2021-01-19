using CaseManagement.HumanTask.Domains;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.InMemory
{
    public class NotificationDefCommandRepository : INotificationDefCommandRepository
    {
        private readonly ConcurrentBag<NotificationDefinitionAggregate> _notificationDefs;

        public NotificationDefCommandRepository(ConcurrentBag<NotificationDefinitionAggregate> notificationDefs)
        {
            _notificationDefs = notificationDefs;
        }

        public Task<bool> Add(NotificationDefinitionAggregate notificationDefinition, CancellationToken token)
        {
            _notificationDefs.Add(notificationDefinition);
            return Task.FromResult(true);
        }

        public Task<bool> Update(NotificationDefinitionAggregate notificationDefinition, CancellationToken token)
        {
            var r = _notificationDefs.First(_ => _.AggregateId == notificationDefinition.AggregateId);
            _notificationDefs.Remove(r);
            _notificationDefs.Add((NotificationDefinitionAggregate)notificationDefinition.Clone());
            return Task.FromResult(true);
        }

        public Task<bool> Delete(string name, CancellationToken token)
        {
            var record = _notificationDefs.FirstOrDefault(_ => _.Name == name);
            if (record == null)
            {
                return Task.FromResult(false);
            }

            _notificationDefs.Remove(record);
            return Task.FromResult(true);
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}
