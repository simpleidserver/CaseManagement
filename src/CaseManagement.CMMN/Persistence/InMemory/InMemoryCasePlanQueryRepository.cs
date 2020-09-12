using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCasePlanQueryRepository : ICasePlanQueryRepository
    {
        private static Dictionary<string, string> MAPPING_WORKFLOWDEFINITION_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "name", "Name" },
            { "create_datetime", "CreateDateTime" },
            { "version", "Version" }
        };
        private readonly ConcurrentBag<CasePlanAggregate> _definitions;

        public InMemoryCasePlanQueryRepository(ConcurrentBag<CasePlanAggregate> definitions)
        {
            _definitions = definitions;
        }

        public Task<CasePlanAggregate> Get(string id, CancellationToken token)
        {
            return Task.FromResult(_definitions.FirstOrDefault(d => d.AggregateId == id));
        }

        public Task<FindResponse<CasePlanAggregate>> Find(FindCasePlansParameter parameter, CancellationToken token)
        {
            IQueryable<CasePlanAggregate> result = _definitions.AsQueryable();
            if (parameter.TakeLatest)
            {
                result = result.OrderByDescending(r => r.Version);
                result = result.GroupBy(r => r.CasePlanId).Select(r => r.First());
            }

            if (MAPPING_WORKFLOWDEFINITION_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_WORKFLOWDEFINITION_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            if (!string.IsNullOrWhiteSpace(parameter.CaseFileId))
            {
                result = result.Where(r => r.CaseFileId == parameter.CaseFileId);
            }

            if (!string.IsNullOrWhiteSpace(parameter.Text))
            {
                result = result.Where(r => r.Name.IndexOf(parameter.Text, StringComparison.InvariantCultureIgnoreCase) >= 0);
            }

            if (!string.IsNullOrWhiteSpace(parameter.CaseOwner))
            {
                result = result.Where(r => r.CaseOwner == parameter.CaseOwner);
            }

            if (!string.IsNullOrWhiteSpace(parameter.CasePlanId))
            {
                result = result.Where(r => r.CasePlanId == parameter.CasePlanId);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<CasePlanAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (ICollection<CasePlanAggregate>)result.ToList()
            });
        }

        public Task<int> Count(CancellationToken token)
        {
            return Task.FromResult(_definitions.Count());
        }
    }
}
