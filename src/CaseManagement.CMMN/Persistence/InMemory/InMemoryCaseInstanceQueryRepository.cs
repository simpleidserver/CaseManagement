using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCaseInstanceQueryRepository : ICasePlanInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "case_plan_id", "CasePlanId" },
            { "state", "State" },
            { "create_datetime", "CreateDateTime" }
        };
        private ConcurrentBag<CasePlanInstanceAggregate> _instances;

        public InMemoryCaseInstanceQueryRepository(ConcurrentBag<CasePlanInstanceAggregate> instances)
        {
            _instances = instances;
        }

        public Task<FindResponse<CasePlanInstanceAggregate>> Find(FindCasePlanInstancesParameter parameter, CancellationToken token)
        {
            IQueryable<CasePlanInstanceAggregate> result = _instances.AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameter.CasePlanId))
            {
                result = result.Where(_ => _.CasePlanId == parameter.CasePlanId);
            }

            if (MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<CasePlanInstanceAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }

        public Task<CasePlanInstanceAggregate> Get(string id, CancellationToken token)
        {
            var result = _instances.FirstOrDefault(i => i.AggregateId == id);
            if (result == null)
            {
                return Task.FromResult((CasePlanInstanceAggregate)null);
            }

            return Task.FromResult(result.Clone() as CasePlanInstanceAggregate);
        }

        public void Dispose()
        {
        }
    }
}
