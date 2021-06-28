using CaseManagement.CMMN.CasePlanInstance.Results;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Results;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF.Persistence
{
    public class CasePlanInstanceQueryRepository : ICasePlanInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "case_plan_id", "CasePlanId" },
            { "state", "State" },
            { "create_datetime", "CreateDateTime" }
        };

        private readonly CaseManagementDbContext _dbContext;

        public CasePlanInstanceQueryRepository(CaseManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SearchResult<CasePlanInstanceResult>> Find(FindCasePlanInstancesParameter parameter, CancellationToken token)
        {
            IQueryable<CasePlanInstanceAggregate> result = _dbContext.CasePlanInstances;
            if (!string.IsNullOrWhiteSpace(parameter.CasePlanId))
            {
                result = result.Where(r => r.CasePlanId == parameter.CasePlanId);
            }

            if (!string.IsNullOrWhiteSpace(parameter.CaseFileId))
            {
                result = result.Where(r => r.CaseFileId == parameter.CaseFileId);
            }

            if (MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            var content = await result.ToListAsync();
            return new SearchResult<CasePlanInstanceResult>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content.Select(_ => CasePlanInstanceResult.ToDto(_)).ToList()
            };
        }

        public async Task<CasePlanInstanceResult> Get(string id, CancellationToken token)
        {
            var result = await _dbContext.CasePlanInstances.FirstOrDefaultAsync(_ => _.AggregateId == id, token);
            if (result == null)
            {
                return null;
            }

            return CasePlanInstanceResult.ToDto(result);
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }
    }
}
