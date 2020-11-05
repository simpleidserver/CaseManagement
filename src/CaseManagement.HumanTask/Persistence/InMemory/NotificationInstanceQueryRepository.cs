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
    public class NotificationInstanceQueryRepository : INotificationInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_NOTIFICATIONINSTANCE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "priority", "Priority" },
            { "createdTime", "CreateDateTime" },
        };
        private readonly ConcurrentBag<NotificationInstanceAggregate> _notifications;

        public NotificationInstanceQueryRepository(ConcurrentBag<NotificationInstanceAggregate> notifications)
        {
            _notifications = notifications;
        }

        public Task<NotificationInstanceAggregate> Get(string id, CancellationToken token)
        {
            return Task.FromResult(_notifications.FirstOrDefault(_ => _.AggregateId == id));
        }

        public Task<FindResponse<NotificationInstanceAggregate>> Find(FindNotificationInstanceParameter parameter, CancellationToken token)
        {
            IQueryable<NotificationInstanceAggregate> result = _notifications.Where(n =>
            {
                return n.PeopleAssignments.Any(p =>
                {
                    if (p.Usage != PeopleAssignmentUsages.RECIPIENT)
                    {
                        return false;
                    }

                    switch (p.Type)
                    {
                        case PeopleAssignmentTypes.USERIDENTIFIERS:
                            return p.Value == parameter.User.UserIdentifier;
                        case PeopleAssignmentTypes.LOGICALPEOPLEGROUP:
                            return parameter.User.LogicalGroups.Contains(p.Value);
                        case PeopleAssignmentTypes.GROUPNAMES:
                            return parameter.User.Roles.Contains(p.Value);
                    }

                    return false;
                });
            }).AsQueryable();
            int totalLength = result.Count();
            if (MAPPING_NOTIFICATIONINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_NOTIFICATIONINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<NotificationInstanceAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (ICollection<NotificationInstanceAggregate>)result.ToList()
            });
        }
    }
}
