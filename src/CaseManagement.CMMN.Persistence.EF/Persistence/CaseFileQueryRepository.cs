using CaseManagement.CMMN.CaseFile.Results;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF.Persistence
{
    public class CaseFileQueryRepository : ICaseFileQueryRepository
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
        private readonly CaseManagementDbContext _caseManagementDbContext;

        public CaseFileQueryRepository(CaseManagementDbContext caseManagementDbContext)
        {
            _caseManagementDbContext = caseManagementDbContext;
        }

        public async Task<CaseFileResult> Get(string id, CancellationToken token)
        {
            var result = await _caseManagementDbContext.CaseFiles.FirstOrDefaultAsync(c => c.AggregateId == id, token);
            if (result == null)
            {
                return null;
            }

            return CaseFileResult.ToDto(result);
        }

        public async Task<int> Count(CancellationToken token)
        {
            var result = await _caseManagementDbContext.CaseFiles.CountAsync(token);
            return result;
        }

        public async Task<SearchResult<CaseFileResult>> Find(FindCaseFilesParameter parameter, CancellationToken token)
        {
            IQueryable<CaseFileAggregate> result = _caseManagementDbContext.CaseFiles;
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
            var content = await result.ToListAsync(token);
            if (parameter.TakeLatest)
            {
                content = content.OrderByDescending(r => r.Version).GroupBy(r => r.FileId).Select(r => r.First()).ToList();
            }

            return new SearchResult<CaseFileResult>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content.Select(_ => CaseFileResult.ToDto(_)).ToList()
            };
        }
    }
}
