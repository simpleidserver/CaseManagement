using CaseManagement.Common.Responses;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence.Parameters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.InMemory
{
    public class HumanTaskInstanceQueryRepository : IHumanTaskInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_HUMANTASKINSTANCE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "priority", "Priority" },
            { "createdTime", "CreateDateTime" },
        };
        private readonly ConcurrentBag<HumanTaskInstanceAggregate> _humanTaskInstances;

        public HumanTaskInstanceQueryRepository(ConcurrentBag<HumanTaskInstanceAggregate> humanTaskInstances)
        {
            _humanTaskInstances = humanTaskInstances;
        }

        public Task<HumanTaskInstanceAggregate> Get(string id, CancellationToken token)
        {
            return Task.FromResult((HumanTaskInstanceAggregate)_humanTaskInstances.FirstOrDefault(_ => _.AggregateId == id)?.Clone());
        }

        public Task<ICollection<HumanTaskInstanceAggregate>> GetSubTasks(string parentHumanTaskId, CancellationToken token)
        {
            ICollection<HumanTaskInstanceAggregate> result = _humanTaskInstances.Where(_ => _.ParentHumanTaskId == parentHumanTaskId).Select(_ => (HumanTaskInstanceAggregate)_.Clone()).ToList();
            return Task.FromResult(result);
        }

        public Task<ICollection<HumanTaskInstanceAggregate>> GetPendingLst(CancellationToken token)
        {
            ICollection<HumanTaskInstanceAggregate> result = _humanTaskInstances.Where(_ => _.ActivationDeferralTime != null && _.ActivationDeferralTime <= DateTime.UtcNow && _.Status == HumanTaskInstanceStatus.CREATED).Select(_ => (HumanTaskInstanceAggregate)_.Clone()).ToList();
            return Task.FromResult(result);
        }

        public Task<ICollection<HumanTaskInstanceAggregate>> GetPendingDeadLines(CancellationToken token, DateTime currentDateTime)
        {
            ICollection<HumanTaskInstanceAggregate> result = _humanTaskInstances.Where(_ => _.DeadLines.Any(d => currentDateTime >= d.EndDateTime)).ToList();
            return Task.FromResult(result);
        }

        public Task<FindResponse<HumanTaskInstanceEventHistory>> FindHumanTaskInstanceHistory(FindHumanTaskInstanceHistoryParameter parameter, CancellationToken token)
        {
            var result = _humanTaskInstances.FirstOrDefault(_ => _.AggregateId == parameter.HumanTaskInstanceId);
            if (result == null)
            {
                return Task.FromResult((FindResponse<HumanTaskInstanceEventHistory>)null);
            }

            int totalLength = result.EventHistories.Count();
            var filtered = result.EventHistories.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<HumanTaskInstanceEventHistory>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = filtered.ToList()
            });
        }

        public Task<FindResponse<HumanTaskInstanceAggregate>> Search(SearchHumanTaskInstanceParameter parameter, CancellationToken token)
        {
            IQueryable<HumanTaskInstanceAggregate> content = _humanTaskInstances.AsQueryable();
            if (parameter.StatusLst != null && parameter.StatusLst.Any())
            {
                content = content.Where(_ => parameter.StatusLst.Contains(_.Status));
            }
            
            if (!string.IsNullOrWhiteSpace(parameter.ActualOwner))
            {
                content = content.Where(_ => !string.IsNullOrWhiteSpace(_.ActualOwner) && _.ActualOwner.StartsWith(parameter.ActualOwner, StringComparison.InvariantCultureIgnoreCase));
            }

            content = content.Where(_ => (IsAuthorized(_.PotentialOwners, parameter.UserIdentifier, parameter.GroupNames) ||
                IsAuthorized(_.BusinessAdministrators, parameter.UserIdentifier, parameter.GroupNames) ||
                IsAuthorized(_.TaskInitiators, parameter.UserIdentifier, parameter.GroupNames)));
            if (MAPPING_HUMANTASKINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                content = content.InvokeOrderBy(MAPPING_HUMANTASKINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = content.Count();
            var result = content.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<HumanTaskInstanceAggregate>
            {
                Content = result.ToList(),
                Count = parameter.Count,
                StartIndex = parameter.StartIndex,
                TotalLength = totalLength
            });
        }

        private static bool IsAuthorized(ICollection<PeopleAssignmentInstance> peopleAssignments, string userIdentifier, ICollection<string> groupNames)
        {
            foreach(var peopleAssignment in peopleAssignments)
            {
                switch(peopleAssignment.Type)
                {
                    case PeopleAssignmentTypes.GROUPNAMES:
                        if (groupNames.Contains(peopleAssignment.Value))
                        {
                            return true;
                        }
                        break;
                    case PeopleAssignmentTypes.USERIDENTIFIERS:
                        if (peopleAssignment.Value == userIdentifier)
                        {
                            return true;
                        }
                        break;
                }
            }

            return false;
        }
    }
}