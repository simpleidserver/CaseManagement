using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.BPMN.ProcessFile.Results;
using CaseManagement.Common.Results;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.InMemory
{
    public class InMemoryProcessFileQueryRepository : IProcessFileQueryRepository
    {
        private static Dictionary<string, string> MAPPING_PROCESSFILE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "AggregateId" },
            { "name", "Name" },
            { "description", "Description" },
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" },
            { "version", "Version" }
        };
        private readonly ConcurrentBag<ProcessFileAggregate> _processFiles;

        public InMemoryProcessFileQueryRepository(ConcurrentBag<ProcessFileAggregate> processFiles)
        {
            _processFiles = processFiles;
        }

        public Task<ProcessFileResult> Get(string id, CancellationToken token)
        {
            var processFile = _processFiles.FirstOrDefault(_ => _.AggregateId == id);
            var result = processFile == null ? null : ProcessFileResult.ToDto(processFile);
            return Task.FromResult(result);
        }

        public Task<SearchResult<ProcessFileResult>> Find(FindProcessFilesParameter parameter, CancellationToken token)
        {
            IQueryable<ProcessFileAggregate> result = _processFiles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameter.FileId))
            {
                result = result.Where(_ => _.FileId == parameter.FileId);
            }

            if (parameter.TakeLatest)
            {
                result = result.OrderByDescending(r => r.Version);
                result = result.GroupBy(r => r.FileId).Select(r => r.First());
            }

            if (MAPPING_PROCESSFILE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_PROCESSFILE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new SearchResult<ProcessFileResult>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.Select(_ => ProcessFileResult.ToDto(_)).ToList()
            });
        }
    }
}
