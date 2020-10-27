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

        public Task<ICollection<HumanTaskInstanceAggregate>> GetPendingDeadLines(CancellationToken token)
        {
            var currentDateTime = DateTime.UtcNow;
            ICollection<HumanTaskInstanceAggregate> result = _humanTaskInstances.Where(_ => _.DeadLines.Any(d => currentDateTime >= d.EndDateTime)
            ).ToList();
            foreach(var record in result)
            {
                record.DeadLines = record.DeadLines.Where(d => currentDateTime >= d.EndDateTime).ToList();
            }

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
            IEnumerable<HumanTaskInstanceAggregate> content = _humanTaskInstances;
            if (parameter.StatusLst != null && parameter.StatusLst.Any())
            {
                content = _humanTaskInstances.Where(_ => parameter.StatusLst.Contains(_.Status));
            }

            content = content.Where(_ => _.PeopleAssignment != null &&
                (IsAuthorized(_.PeopleAssignment.PotentialOwner, parameter.UserIdentifier, parameter.GroupNames) ||
                IsAuthorized(_.PeopleAssignment.BusinessAdministrator, parameter.UserIdentifier, parameter.GroupNames) ||
                IsAuthorized(_.PeopleAssignment.TaskInitiator, parameter.UserIdentifier, parameter.GroupNames))).ToList();
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

        private static bool IsAuthorized(PeopleAssignmentInstance peopleAssignment, string userIdentifier, ICollection<string> groupNames)
        {
            if (peopleAssignment == null)
            {
                return false;
            }

            switch(peopleAssignment.Type)
            {
                case PeopleAssignmentTypes.GROUPNAMES:
                    return peopleAssignment.Values.Any(_ => groupNames.Contains(_));
                case PeopleAssignmentTypes.USERIDENTIFIERS:
                    return peopleAssignment.Values.Contains(userIdentifier);
            }

            return false;
        }
    }
}