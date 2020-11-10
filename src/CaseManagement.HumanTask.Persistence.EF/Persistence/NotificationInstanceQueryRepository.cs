using CaseManagement.Common.Responses;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence.Parameters;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.EF.Persistence
{
    public class NotificationInstanceQueryRepository : INotificationInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_NOTIFICATIONINSTANCE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "priority", "Priority" },
            { "createdTime", "CreateDateTime" },
        };
        private readonly HumanTaskDBContext _dbContext;

        public NotificationInstanceQueryRepository(HumanTaskDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<FindResponse<NotificationInstanceAggregate>> Find(FindNotificationInstanceParameter parameter, CancellationToken token)
        {
            IQueryable<NotificationInstanceAggregate> result = _dbContext.NotificationInstanceAggregate.Include(_ => _.PeopleAssignments)
                .Include(_ => _.PresentationElements)
                .Where(_ => _.PeopleAssignments.Any(p =>
                (p.Usage == PeopleAssignmentUsages.RECIPIENT && p.Type == PeopleAssignmentTypes.USERIDENTIFIERS && p.Value == parameter.User.UserIdentifier) ||
                (p.Usage == PeopleAssignmentUsages.BUSINESSADMINISTRATOR && p.Type == PeopleAssignmentTypes.USERIDENTIFIERS && p.Value == parameter.User.UserIdentifier) ||
                (p.Usage == PeopleAssignmentUsages.BUSINESSADMINISTRATOR && p.Type == PeopleAssignmentTypes.GROUPNAMES && parameter.User.Roles.Contains(p.Value)) ||
                (p.Usage == PeopleAssignmentUsages.RECIPIENT && p.Type == PeopleAssignmentTypes.GROUPNAMES && parameter.User.Roles.Contains(p.Value))));
            int totalLength = await result.CountAsync(token);
            if (MAPPING_NOTIFICATIONINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_NOTIFICATIONINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            var content = await result.ToListAsync(token);
            return new FindResponse<NotificationInstanceAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content
            };
        }

        public Task<NotificationInstanceAggregate> Get(string id, CancellationToken token)
        {
            return _dbContext.NotificationInstanceAggregate.Include(_ => _.PeopleAssignments)
                .Include(_ => _.PresentationElements)
                .FirstOrDefaultAsync(_ => _.AggregateId == id, token);
        }
    }
}
