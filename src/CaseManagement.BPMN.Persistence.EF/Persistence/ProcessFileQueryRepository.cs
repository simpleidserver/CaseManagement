using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.EF.DomainMapping;
using CaseManagement.BPMN.Persistence.EF.Models;
using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
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

        public async Task<ProcessFileAggregate> Get(string id, CancellationToken token)
        {
            var result = await _dbContext.ProcessFiles.FirstOrDefaultAsync(_ => _.Id == id);
            return result?.ToDomain();
        }

        public async Task<FindResponse<ProcessFileAggregate>> Find(FindProcessFilesParameter parameter, CancellationToken token)
        {
            IQueryable<ProcessFileModel> result = _dbContext.ProcessFiles.AsQueryable();
            if (parameter.TakeLatest)
            {
                result = result.OrderByDescending(r => r.Version);
                result = result.GroupBy(r => r.FileId).Select(r => r.First());
            }

            if (MAPPING_PROCESSFILE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_PROCESSFILE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = await result.CountAsync(token);
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return new FindResponse<ProcessFileAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (await result.ToListAsync(token)).Select(_ => _.ToDomain()).ToList()
            };
        }
    }
}
