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
    public class InMemoryCMMNWorkflowInstanceQueryRepository : ICMMNWorkflowInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "case_definition_id", "WorkflowDefinitionId" },
            { "state", "State" },
            { "create_datetime", "CreateDateTime" },
        };
        private ConcurrentBag<CMMNWorkflowInstance> _instances;

        public InMemoryCMMNWorkflowInstanceQueryRepository(ConcurrentBag<CMMNWorkflowInstance> instances)
        {
            _instances = instances;
        }

        public Task<FindResponse<CMMNWorkflowInstance>> Find(FindWorkflowInstanceParameter parameter)
        {
            IQueryable<CMMNWorkflowInstance> result = _instances.AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameter.CaseDefinitionId))
            {
                result = result.Where(r => r.WorkflowDefinitionId == parameter.CaseDefinitionId);
            }

            if (MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<CMMNWorkflowInstance>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (ICollection<CMMNWorkflowInstance>)result.ToList()
            });
        }

        public Task<CMMNWorkflowInstance> FindFlowInstanceById(string id)
        {
            var result = _instances.FirstOrDefault(i => i.Id == id);
            if (result == null)
            {
                return Task.FromResult((CMMNWorkflowInstance)null);
            }

            return Task.FromResult(result.Clone() as CMMNWorkflowInstance);
        }     
    }
}
