using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryWorkflowInstanceQueryRepository : IWorkflowInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "case_definition_id", "CaseDefinitionId" },
            { "state", "State" },
            { "create_datetime", "CreateDateTime" },
        };
        private ConcurrentBag<Domains.CaseInstance> _instances;

        public InMemoryWorkflowInstanceQueryRepository(ConcurrentBag<Domains.CaseInstance> instances)
        {
            _instances = instances;
        }

        public Task<FindResponse<Domains.CaseInstance>> Find(FindWorkflowInstanceParameter parameter)
        {
            IQueryable<Domains.CaseInstance> result = _instances.AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameter.CaseDefinitionId))
            {
                result = result.Where(r => r.CaseDefinitionId == parameter.CaseDefinitionId);
            }

            if (MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<Domains.CaseInstance>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (ICollection<Domains.CaseInstance>)result.ToList()
            });
        }

        public Task<Domains.CaseInstance> FindFlowInstanceById(string id)
        {
            var result = _instances.FirstOrDefault(i => i.Id == id);
            if (result == null)
            {
                return Task.FromResult((Domains.CaseInstance)null);
            }

            return Task.FromResult(result.Clone() as Domains.CaseInstance);
        }     
    }
}
