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
    public class InMemoryCaseFileQueryRepository : ICaseFileQueryRepository
    {
        private static Dictionary<string, string> MAPPING_WORKFLOWDEFINITIONFILE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "name", "Name" },
            { "description", "Description" },
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };
        private ConcurrentBag<CaseFileAggregate> _caseFileDefinitions;

        public InMemoryCaseFileQueryRepository(ConcurrentBag<CaseFileAggregate> caseFileDefinitions)
        {
            _caseFileDefinitions = caseFileDefinitions;
        }

        public Task<CaseFileAggregate> FindById(string id)
        {
            return Task.FromResult(_caseFileDefinitions.FirstOrDefault(c => c.Id == id));
        }

        public Task<FindResponse<CaseFileAggregate>> Find(FindCaseFilesParameter parameter)
        {
            IQueryable<CaseFileAggregate> result = _caseFileDefinitions.AsQueryable();
            if (MAPPING_WORKFLOWDEFINITIONFILE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_WORKFLOWDEFINITIONFILE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            if (!string.IsNullOrWhiteSpace(parameter.Owner))
            {
                result = result.Where(r => r.Owner == parameter.Owner);
            }

            if (!string.IsNullOrWhiteSpace(parameter.Text))
            {
                result = result.Where(r => r.Name.IndexOf(parameter.Text, StringComparison.InvariantCultureIgnoreCase) >= 0);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<CaseFileAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (ICollection<CaseFileAggregate>)result.ToList()
            });
        }

        public Task<int> Count()
        {
            return Task.FromResult(_caseFileDefinitions.Count());
        }
    }
}
