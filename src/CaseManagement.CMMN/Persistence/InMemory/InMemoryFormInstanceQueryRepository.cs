using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryFormInstanceQueryRepository : IFormInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_FORMINSTANCENAME_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };

        private readonly List<FormInstanceAggregate> _formInstances;

        public InMemoryFormInstanceQueryRepository(List<FormInstanceAggregate> formInstances)
        {
            _formInstances = formInstances;
        }

        public Task<FindResponse<FormInstanceAggregate>> Find(FindFormInstanceParameter parameter)
        {
            IQueryable<FormInstanceAggregate> result = _formInstances.Where(fi => parameter.RoleIds.Contains(fi.RoleId)).AsQueryable();
            /*
            if (MAPPING_FORMINSTANCENAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = InMemoryProcessFlowInstanceQueryRepository.InvokeOrderBy(result, MAPPING_FORMINSTANCENAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }
            */

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
