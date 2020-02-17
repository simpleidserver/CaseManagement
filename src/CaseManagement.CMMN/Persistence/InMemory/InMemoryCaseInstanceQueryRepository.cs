using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
            { "create_datetime", "CreateDateTime" },
        };
        private ConcurrentBag<Domains.CasePlanInstanceAggregate> _instances;

        public InMemoryCaseInstanceQueryRepository(ConcurrentBag<Domains.CasePlanInstanceAggregate> instances)
        {
            _instances = instances;
        }

        public Task<FindResponse<Domains.CasePlanInstanceAggregate>> Find(FindWorkflowInstanceParameter parameter)
        {
            IQueryable<Domains.CasePlanInstanceAggregate> result = _instances.AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameter.CasePlanId))
            {
                result = result.Where(r => r.CasePlanId == parameter.CasePlanId);
            }

            if (MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<Domains.CasePlanInstanceAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (ICollection<Domains.CasePlanInstanceAggregate>)result.ToList()
            });
        }

        public Task<Domains.CasePlanInstanceAggregate> FindFlowInstanceById(string id)
        {
            var result = _instances.FirstOrDefault(i => i.Id == id);
            if (result == null)
            {
                return Task.FromResult((Domains.CasePlanInstanceAggregate)null);
            }

            return Task.FromResult(result.Clone() as Domains.CasePlanInstanceAggregate);
        }     
    }
}
