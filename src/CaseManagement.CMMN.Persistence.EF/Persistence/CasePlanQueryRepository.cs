using CaseManagement.CMMN.CasePlan.Results;
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
    public class CasePlanQueryRepository : ICasePlanQueryRepository
    {
        private static Dictionary<string, string> MAPPING_WORKFLOWDEFINITION_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "name", "Name" },
            { "create_datetime", "CreateDateTime" },
            { "version", "Version" }
        };
        private readonly CaseManagementDbContext _dbContext;

        public CasePlanQueryRepository(CaseManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SearchResult<CasePlanResult>> Find(FindCasePlansParameter parameter, CancellationToken token)
        {
            IQueryable<CasePlanAggregate> result = _dbContext.CasePlans;
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

            if (!string.IsNullOrWhiteSpace(parameter.CasePlanId))
            {
                result = result.Where(r => r.CasePlanId == parameter.CasePlanId);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            var content = await result.ToListAsync(token);
            if (parameter.TakeLatest)
            {
                content = content.OrderByDescending(r => r.Version).GroupBy(r => r.CasePlanId).Select(r => r.First()).ToList();
            }

            return new SearchResult<CasePlanResult>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content.Select(_ => CasePlanResult.ToDto(_)).ToList()
            };
        }

        public async Task<CasePlanResult> Get(string id, CancellationToken token)
        {
            var result = await _dbContext.CasePlans
                .FirstOrDefaultAsync(_ => _.AggregateId == id, token);
            if (result == null)
            {
                return null;
            }

            return CasePlanResult.ToDto(result);
        }

        public Task<int> Count(CancellationToken token)
        {
            return _dbContext.CasePlans.CountAsync(token);
        }
    }
}
