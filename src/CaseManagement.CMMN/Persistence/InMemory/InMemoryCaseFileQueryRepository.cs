using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            { "update_datetime", "UpdateDateTime" },
            { "version", "Version" }
        };
        private ConcurrentBag<CaseFileAggregate> _caseFileDefinitions;

        public InMemoryCaseFileQueryRepository(ConcurrentBag<CaseFileAggregate> caseFileDefinitions)
        {
            _caseFileDefinitions = caseFileDefinitions;
        }

        public Task<CaseFileAggregate> Get(string id, CancellationToken token)
        {
            return Task.FromResult(_caseFileDefinitions.FirstOrDefault(c => c.AggregateId == id));
        }

        public Task<SearchResult<CaseFileAggregate>> Find(FindCaseFilesParameter parameter, CancellationToken token)
        {
            IQueryable<CaseFileAggregate> result = _caseFileDefinitions.AsQueryable();
            if (parameter.TakeLatest)
            {
                result = result.OrderByDescending(r => r.Version);
                result = result.GroupBy(r => r.FileId).Select(r => r.First());
            }

            if (!string.IsNullOrWhiteSpace(parameter.Text))
            {
                result = result.Where(r => r.Name.IndexOf(parameter.Text, StringComparison.InvariantCultureIgnoreCase) >= 0);
            }

            if (!string.IsNullOrWhiteSpace(parameter.CaseFileId))
            {
                result = result.Where(r => r.FileId == parameter.CaseFileId);
            }

            if (MAPPING_WORKFLOWDEFINITIONFILE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_WORKFLOWDEFINITIONFILE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new SearchResult<CaseFileAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (ICollection<CaseFileAggregate>)result.ToList()
            });
        }

        public Task<int> Count(CancellationToken token)
        {
            return Task.FromResult(_caseFileDefinitions.Where(c => c.Status == CaseFileStatus.Published).Count());
        }
    }
}
