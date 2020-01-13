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
    public class InMemoryActivationQueryRepository : IActivationQueryRepository
    {
        private static Dictionary<string, string> MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "case_instance_id", "CaseInstanceId" },
            { "case_instance_name", "WorkflowInstanceName" },
            { "case_element_name", "WorkflowElementName" },
            { "performer", "Performer" },
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };

        private ConcurrentBag<CaseActivationAggregate> _activations;

        public InMemoryActivationQueryRepository(ConcurrentBag<CaseActivationAggregate> activations)
        {
            _activations = activations;
        }

        public Task<CaseActivationAggregate> FindById(string id)
        {
            return Task.FromResult(_activations.FirstOrDefault(a => a.WorkflowElementInstanceId == id));
        }

        public Task<FindResponse<CaseActivationAggregate>> Find(FindCaseActivationsParameter parameter)
        {
            IQueryable<CaseActivationAggregate> result = _activations.AsQueryable();
            if (MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }
            
            if (!string.IsNullOrWhiteSpace(parameter.CaseDefinitionId))
            {
                result = result.Where(r => r.WorkflowId == parameter.CaseDefinitionId);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<CaseActivationAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }
    }
}
