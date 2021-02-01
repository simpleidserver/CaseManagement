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
    public class NotificationDefQueryRepository : INotificationDefQueryRepository
    {
        private readonly HumanTaskDBContext _dbContext;
        private static Dictionary<string, string> MAPPING_NOTIFICATIONDEF_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" },
        };

        public NotificationDefQueryRepository(HumanTaskDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<NotificationDefinitionAggregate> Get(string id, CancellationToken token)
        {
            var result = await _dbContext.NotificationDefinitions
                .Include(_ => _.OperationParameters)
                .Include(_ => _.PresentationElements)
                .Include(_ => _.PeopleAssignments)
                .Include(_ => _.PresentationParameters)
                .FirstOrDefaultAsync(_ => _.AggregateId == id, token);
            return result;
        }

        public async Task<ICollection<NotificationDefinitionAggregate>> GetAll(CancellationToken token)
        {
            var result = await _dbContext.NotificationDefinitions
                .Include(_ => _.OperationParameters)
                .Include(_ => _.PresentationElements)
                .Include(_ => _.PeopleAssignments)
                .Include(_ => _.PresentationParameters)
                .ToListAsync(token);
            return result;
        }

        public async Task<NotificationDefinitionAggregate> GetLatest(string name, CancellationToken token)
        {
            var result = _dbContext.NotificationDefinitions
                .Include(_ => _.OperationParameters)
                .Include(_ => _.PresentationElements)
                .Include(_ => _.PeopleAssignments)
                .Include(_ => _.PresentationParameters);
            var ordered = result.OrderByDescending(_ => _.Version);
            return await ordered.FirstOrDefaultAsync(_ => _.Name == name, token);
        }

        public async Task<FindResponse<NotificationDefinitionAggregate>> Search(SearchNotificationDefParameter parameter, CancellationToken token)
        {
            IQueryable<NotificationDefinitionAggregate> result = _dbContext.NotificationDefinitions
                .Include(_ => _.OperationParameters)
                .Include(_ => _.PresentationElements)
                .Include(_ => _.PeopleAssignments)
                .Include(_ => _.PresentationParameters); ;
            if (!string.IsNullOrWhiteSpace(parameter.Name))
            {
                result = result.Where(_ => _.Name == parameter.Name);
            }

            if (MAPPING_NOTIFICATIONDEF_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_NOTIFICATIONDEF_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = await result.CountAsync(token);
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return new FindResponse<NotificationDefinitionAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = await result.ToListAsync(token)
            };
        }
    }
}
