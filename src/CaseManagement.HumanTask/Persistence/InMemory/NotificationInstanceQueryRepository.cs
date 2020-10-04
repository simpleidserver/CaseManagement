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
        private readonly ConcurrentBag<NotificationInstanceAggregate> _notifications;

        public NotificationInstanceQueryRepository(ConcurrentBag<NotificationInstanceAggregate> notifications)
        {
            _notifications = notifications;
        }

        public Task<FindResponse<NotificationInstanceAggregate>> Find(FindNotificationInstanceParameter parameter, CancellationToken token)
        {
            var result = _notifications.Where(n =>
            n.PeopleAssignment != null &&
            n.PeopleAssignment.Recipient != null &&
            (
                (n.PeopleAssignment.Recipient.Type == PeopleAssignmentTypes.USERIDENTFIERS && n.PeopleAssignment.Recipient.Values.Contains(parameter.User.UserIdentifier)) ||
                (n.PeopleAssignment.Recipient.Type == PeopleAssignmentTypes.GROUPNAMES && n.PeopleAssignment.Recipient.Values.Any(v => parameter.User.Roles.Contains(v))) ||
                (n.PeopleAssignment.Recipient.Type == PeopleAssignmentTypes.LOGICALPEOPLEGROUP && n.PeopleAssignment.Recipient.LogicalGroup != null && parameter.User.LogicalGroups.Contains(n.PeopleAssignment.Recipient.LogicalGroup.Name))
            ));
            int totalLength = result.Count();
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
