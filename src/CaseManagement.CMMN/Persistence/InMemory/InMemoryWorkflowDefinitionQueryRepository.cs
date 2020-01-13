using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryWorkflowDefinitionQueryRepository : IWorkflowDefinitionQueryRepository
    {
        private static Dictionary<string, string> MAPPING_WORKFLOWDEFINITION_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "name", "Name" },
            { "create_datetime", "CreateDateTime" },
            { "cmmn_definition", "CmmnDefinition" }
        };
        private readonly ICollection<CaseDefinition> _definitions;

        public InMemoryWorkflowDefinitionQueryRepository(ICollection<CaseDefinition> definitions)
        {
            _definitions = definitions;
        }

        public Task<CaseDefinition> FindById(string id)
        {
            return Task.FromResult(_definitions.FirstOrDefault(d => d.Id == id));
        }

        public Task<FindResponse<CaseDefinition>> Find(FindWorkflowDefinitionsParameter parameter)
        {
            IQueryable<CaseDefinition> result = _definitions.AsQueryable();
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
                result = result.Where(r => r.Name.Contains(parameter.Text));
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<CaseDefinition>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (ICollection<CaseDefinition>)result.ToList()
            });
        }
    }
}
