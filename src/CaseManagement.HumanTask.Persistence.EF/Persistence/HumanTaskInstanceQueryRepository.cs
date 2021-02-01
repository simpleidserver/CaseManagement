using CaseManagement.Common.Responses;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence.Parameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.EF.Persistence
{
    public class HumanTaskInstanceQueryRepository : IHumanTaskInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_HUMANTASKINSTANCE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "priority", "Priority" },
            { "createdTime", "CreateDateTime" },
        };
        private readonly HumanTaskDBContext _dbContext;

        public HumanTaskInstanceQueryRepository(HumanTaskDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HumanTaskInstanceAggregate> Get(string id, CancellationToken token)
        {
            var result = await _dbContext.HumanTaskInstanceAggregate
                .Include(_ => _.OperationParameters)
                .Include(_ => _.Completions).ThenInclude(_ => _.CopyLst)
                .Include(_ => _.PresentationElements)
                .Include(_ => _.CallbackOperations)
                .Include(_ => _.DeadLines).ThenInclude(_ => _.Escalations).ThenInclude(_ => _.ToParts)
                .Include(_ => _.PeopleAssignments)
                .Include(_ => _.SubTasks).ThenInclude(_ => _.ToParts)
                .Include(_ => _.EventHistories)
                .FirstOrDefaultAsync(_ => _.AggregateId == id, token);
            return result;
        }

        public async Task<FindResponse<HumanTaskInstanceEventHistory>> FindHumanTaskInstanceHistory(FindHumanTaskInstanceHistoryParameter parameter, CancellationToken token)
        {
            var result = await _dbContext.HumanTaskInstanceAggregate.Include(_ => _.EventHistories)
                .FirstOrDefaultAsync(_ => _.AggregateId == parameter.HumanTaskInstanceId, token);
            if (result == null)
            {
                return null;
            }

            int totalLength = result.EventHistories.Count();
            var filtered = result.EventHistories.Skip(parameter.StartIndex).Take(parameter.Count);
            return new FindResponse<HumanTaskInstanceEventHistory>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = filtered.ToList()
            };
        }

        public async Task<ICollection<HumanTaskInstanceAggregate>> GetPendingDeadLines(CancellationToken token, DateTime currentDateTime)
        {
            ICollection<HumanTaskInstanceAggregate> result = await _dbContext.HumanTaskInstanceAggregate
                .Include(_ => _.DeadLines).ThenInclude(_ => _.Escalations)
                .Where(_ => _.DeadLines.Any(d => currentDateTime >= d.EndDateTime))
                .ToListAsync(token);
            return result;
        }

        public async Task<ICollection<HumanTaskInstanceAggregate>> GetPendingLst(CancellationToken token)
        {
            ICollection<HumanTaskInstanceAggregate> result = await _dbContext.HumanTaskInstanceAggregate
                .Where(_ => _.ActivationDeferralTime != null && _.ActivationDeferralTime <= DateTime.UtcNow && _.Status == HumanTaskInstanceStatus.CREATED)
                .ToListAsync(token);
            return result;
        }

        public async Task<ICollection<HumanTaskInstanceAggregate>> GetSubTasks(string parentHumanTaskId, CancellationToken token)
        {
            ICollection<HumanTaskInstanceAggregate> result = await _dbContext.HumanTaskInstanceAggregate
                .Include(_ => _.SubTasks)
                .Where(_ => _.ParentHumanTaskId == parentHumanTaskId)
                .ToListAsync(token);
            return result;
        }

        public async Task<FindResponse<HumanTaskInstanceAggregate>> Search(SearchHumanTaskInstanceParameter parameter, CancellationToken token)
        {
            IQueryable<HumanTaskInstanceAggregate> content = _dbContext.HumanTaskInstanceAggregate
                .Include(_ => _.PeopleAssignments);
            if (parameter.StatusLst != null && parameter.StatusLst.Any())
            {
                content = content.Where(_ => parameter.StatusLst.Contains(_.Status));
            }

            if (!string.IsNullOrWhiteSpace(parameter.ActualOwner))
            {
                content = content.Where(_ => !string.IsNullOrWhiteSpace(_.ActualOwner) && _.ActualOwner.StartsWith(parameter.ActualOwner, StringComparison.InvariantCultureIgnoreCase));
            }

            var groupNames = parameter.GroupNames.ToList();
            content = content.Where(_ => _.PeopleAssignments.Any(p => 
                (p.Usage == PeopleAssignmentUsages.POTENTIALOWNER && p.Type == PeopleAssignmentTypes.USERIDENTIFIERS && p.Value == parameter.UserIdentifier) ||
                (p.Usage == PeopleAssignmentUsages.BUSINESSADMINISTRATOR && p.Type == PeopleAssignmentTypes.USERIDENTIFIERS && p.Value == parameter.UserIdentifier) ||
                (p.Usage == PeopleAssignmentUsages.TASKINITIATOR && p.Type == PeopleAssignmentTypes.USERIDENTIFIERS && p.Value == parameter.UserIdentifier) ||
                (p.Usage == PeopleAssignmentUsages.POTENTIALOWNER && p.Type == PeopleAssignmentTypes.GROUPNAMES && parameter.GroupNames.Contains(p.Value)) ||
                (p.Usage == PeopleAssignmentUsages.BUSINESSADMINISTRATOR && p.Type == PeopleAssignmentTypes.GROUPNAMES && parameter.GroupNames.Contains(p.Value)) ||
                (p.Usage == PeopleAssignmentUsages.TASKINITIATOR && p.Type == PeopleAssignmentTypes.GROUPNAMES && parameter.GroupNames.Contains(p.Value))));

            if (MAPPING_HUMANTASKINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                content = content.InvokeOrderBy(MAPPING_HUMANTASKINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = content.Count();
            var result = content.Skip(parameter.StartIndex).Take(parameter.Count);
            return new FindResponse<HumanTaskInstanceAggregate>
            {
                Content = await result.ToListAsync(token),
                Count = parameter.Count,
                StartIndex = parameter.StartIndex,
                TotalLength = totalLength
            };
        }
    }
}
