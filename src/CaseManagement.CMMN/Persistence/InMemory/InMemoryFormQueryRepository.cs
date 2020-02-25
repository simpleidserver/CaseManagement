using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryFormQueryRepository : IFormQueryRepository
    {
        private static Dictionary<string, string> MAPPING_FORM_TO_PROPERTY_NAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "form_id", "FormId" },
            { "status", "Status" },
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };
        private ICollection<FormAggregate> _forms;

        public InMemoryFormQueryRepository(ICollection<FormAggregate> forms)
        {
            _forms = forms;
        }

        public Task<FindResponse<FormAggregate>> Find(FindFormParameter parameter)
        {
            IQueryable<FormAggregate> result = _forms.AsQueryable();
            if (parameter.Ids != null && parameter.Ids.Any())
            {
                result = result.Where(r => parameter.Ids.Contains(r.Id));
            }

            if (MAPPING_FORM_TO_PROPERTY_NAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_FORM_TO_PROPERTY_NAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<FormAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }

        public Task<FormAggregate> FindFormById(string id)
        {
            return Task.FromResult(_forms.FirstOrDefault(f => f.Id == id));
        }

        public Task<FormAggregate> FindLatestVersion(string formId)
        {
            return Task.FromResult(_forms.OrderByDescending(f => f.Version).FirstOrDefault(f => f.FormId == formId));
        }
    }
}
