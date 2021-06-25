using CaseManagement.Common.Responses;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence.Parameters;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.InMemory
{
    public class NotificationDefQueryRepository : INotificationDefQueryRepository
    {
        private readonly ConcurrentBag<NotificationDefinitionAggregate> _notifications;
        private static Dictionary<string, string> MAPPING_NOTIFICATIONDEF_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" },
        };

        public NotificationDefQueryRepository(ConcurrentBag<NotificationDefinitionAggregate> notifications)
        {
            _notifications = notifications;
        }

        public Task<NotificationDefinitionAggregate> Get(string id, CancellationToken token)
        {
            return Task.FromResult((NotificationDefinitionAggregate)_notifications.FirstOrDefault(_ => _.AggregateId == id)?.Clone());
        }

        public Task<NotificationDefinitionAggregate> GetLatest(string name, CancellationToken token)
        {
            return Task.FromResult((NotificationDefinitionAggregate)_notifications.OrderByDescending(_ => _.Version).FirstOrDefault(_ => _.Name == name)?.Clone());
        }

        public Task<SearchResult<NotificationDefinitionAggregate>> Search(SearchNotificationDefParameter parameter, CancellationToken token)
        {
            IQueryable<NotificationDefinitionAggregate> result = _notifications.AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameter.Name))
            {
                result = result.Where(_ => _.Name == parameter.Name);
            }

            if (MAPPING_NOTIFICATIONDEF_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_NOTIFICATIONDEF_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new SearchResult<NotificationDefinitionAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }

        public Task<ICollection<NotificationDefinitionAggregate>> GetAll(CancellationToken token)
        {
            return Task.FromResult((ICollection<NotificationDefinitionAggregate>)_notifications.ToList());
        }
    }
}
