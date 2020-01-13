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
    public class InMemoryFormInstanceQueryRepository : IFormInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_FORMINSTANCENAME_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "form_id", "FormId" },
            { "performer", "RoleId" },
            { "case_instance_id", "CaseInstanceId" },
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };

        private readonly ConcurrentBag<FormInstanceAggregate> _formInstances;

        public InMemoryFormInstanceQueryRepository(ConcurrentBag<FormInstanceAggregate> formInstances)
        {
            _formInstances = formInstances;
        }

        public Task<FindResponse<FormInstanceAggregate>> Find(FindFormInstanceParameter parameter)
        {
            IQueryable<FormInstanceAggregate> result = _formInstances.AsQueryable();
            if (MAPPING_FORMINSTANCENAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_FORMINSTANCENAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            if (parameter.RoleIds != null && parameter.RoleIds.Any())
            {
                result = result.Where(fi => parameter.RoleIds.Contains(fi.RoleId));
            }

            if (!string.IsNullOrWhiteSpace(parameter.CaseDefinitionId))
            {
                result = result.Where(c => c.CaseDefinitionId == parameter.CaseDefinitionId);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<FormInstanceAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }

        public Task<FormInstanceAggregate> FindById(string id)
        {
            return Task.FromResult(_formInstances.FirstOrDefault(f => f.Id == id));
        }
    }
}
