using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.EF.DomainMapping;
using CaseManagement.CMMN.Persistence.EF.Models;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
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

        public async Task<FindResponse<CasePlanInstanceAggregate>> Find(FindCasePlanInstancesParameter parameter, CancellationToken token)
        {
            using (var lck = await _dbContext.Lock())
            {
                IQueryable<CasePlanInstanceModel> result = _dbContext.CasePlanInstances
                    .Include(_ => _.Roles).ThenInclude(_ => _.Claims)
                    .Include(_ => _.Files)
                    .Include(_ => _.WorkerTasks)
                    .Include(_ => _.Children).ThenInclude(_ => _.Children);
                if (!string.IsNullOrWhiteSpace(parameter.CasePlanId))
                {
                    result = result.Where(r => r.CasePlanId == parameter.CasePlanId);
                }

                if (MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
                {
                    result = result.InvokeOrderBy(MAPPING_WORKFLOWINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
                }

                int totalLength = result.Count();
                result = result.Skip(parameter.StartIndex).Take(parameter.Count);
                var content = await result.ToListAsync();
                return new FindResponse<CasePlanInstanceAggregate>
                {
                    StartIndex = parameter.StartIndex,
                    Count = parameter.Count,
                    TotalLength = totalLength,
                    Content = content.Select(_ => _.ToDomain()).ToList()
                };
            }
        }

        public async Task<CasePlanInstanceAggregate> Get(string id, CancellationToken token)
        {
            using (var lck = await _dbContext.Lock())
            {
                var result = await _dbContext.CasePlanInstances
                    .Include(_ => _.Roles).ThenInclude(_ => _.Claims)
                    .Include(_ => _.Files)
                    .Include(_ => _.WorkerTasks)
                    .Include(_ => _.Children).ThenInclude(_ => _.Children)
                    .FirstOrDefaultAsync(_ => _.Id == id, token);
                if (result == null)
                {
                    return null;
                }

                return result.ToDomain();
            }
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
