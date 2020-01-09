using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCMMNWorkflowDefinitionQueryRepository : ICMMNWorkflowDefinitionQueryRepository
    {
        private static Dictionary<string, string> MAPPING_WORKFLOWDEFINITION_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "name", "Name" },
            { "create_datetime", "CreateDateTime" },
            { "cmmn_definition", "CmmnDefinition" }
        };
        private readonly ICollection<CMMNWorkflowDefinition> _definitions;

        public InMemoryCMMNWorkflowDefinitionQueryRepository(ICollection<CMMNWorkflowDefinition> definitions)
        {
            _definitions = definitions;
        }

        public Task<CMMNWorkflowDefinition> FindById(string id)
        {
            return Task.FromResult(_definitions.FirstOrDefault(d => d.Id == id));
        }

        public Task<FindResponse<CMMNWorkflowDefinition>> Find(FindWorkflowDefinitionsParameter parameter)
        {
            IQueryable<CMMNWorkflowDefinition> result = _definitions.AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameter.CaseFileId))
            {
                result = result.Where(r => r.CaseFileId == parameter.CaseFileId);
            }

            if (MAPPING_WORKFLOWDEFINITION_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_WORKFLOWDEFINITION_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<CMMNWorkflowDefinition>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (ICollection<CMMNWorkflowDefinition>)result.ToList()
            });
        }
    }
}
