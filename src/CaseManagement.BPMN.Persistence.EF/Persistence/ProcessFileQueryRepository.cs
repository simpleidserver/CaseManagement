using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.BPMN.ProcessFile.Results;
using CaseManagement.Common.Results;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.EF.Persistence
{
    public class ProcessFileQueryRepository : IProcessFileQueryRepository
    {
        private static Dictionary<string, string> MAPPING_PROCESSFILE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "name", "Name" },
            { "description", "Description" },
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" },
            { "version", "Version" }
        };
        private readonly BPMNDbContext _dbContext;

        public ProcessFileQueryRepository(BPMNDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProcessFileResult> Get(string id, CancellationToken cancellationToken)
        {
            var processFile = await _dbContext.ProcessFiles.FirstOrDefaultAsync(_ => _.AggregateId == id, cancellationToken);
            return processFile == null ? null : ProcessFileResult.ToDto(processFile);
        }

        public async Task<SearchResult<ProcessFileResult>> Find(FindProcessFilesParameter parameter, CancellationToken token)
        {
            IQueryable<ProcessFileAggregate> result = _dbContext.ProcessFiles;
            if (MAPPING_PROCESSFILE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_PROCESSFILE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = await result.CountAsync(token);
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            var content = await result.ToListAsync(token);
            if (parameter.TakeLatest)
            {
                content = content.OrderByDescending(r => r.Version).GroupBy(r => r.FileId).Select(r => r.First()).ToList();
            }

            return new SearchResult<ProcessFileResult>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content.Select(_ => ProcessFileResult.ToDto(_)).ToList()
            };
        }
    }
}
