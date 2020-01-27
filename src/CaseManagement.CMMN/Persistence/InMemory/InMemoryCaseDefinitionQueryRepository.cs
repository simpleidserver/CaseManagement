using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCaseDefinitionQueryRepository : ICaseDefinitionQueryRepository
    {
        private static Dictionary<string, string> MAPPING_WORKFLOWDEFINITION_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "name", "Name" },
            { "create_datetime", "CreateDateTime" },
            { "cmmn_definition", "CmmnDefinition" }
        };
        private readonly ConcurrentBag<CaseDefinition> _definitions;
        private readonly ConcurrentBag<CaseDefinitionHistoryAggregate> _caseDefinitionHistories;

        public InMemoryCaseDefinitionQueryRepository(ConcurrentBag<CaseDefinition> definitions, ConcurrentBag<CaseDefinitionHistoryAggregate> caseDefinitionHistories)
        {
            _definitions = definitions;
            _caseDefinitionHistories = caseDefinitionHistories;
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
                result = result.Where(r => r.Name.IndexOf(parameter.Text, StringComparison.InvariantCultureIgnoreCase) >= 0);
            }

            if (!string.IsNullOrWhiteSpace(parameter.CaseOwner))
            {
                result = result.Where(r => r.CaseOwner == parameter.CaseOwner);
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

        public Task<CaseDefinitionHistoryAggregate> FindHistoryById(string id)
        {
            return Task.FromResult(_caseDefinitionHistories.FirstOrDefault(w => w.CaseDefinitionId == id));
        }

        public Task<int> Count()
        {
            return Task.FromResult(_definitions.Count());
        }
    }
}
