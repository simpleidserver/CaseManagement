using CaseManagement.CMMN.CaseWorkerTask.Results;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Results;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF.Persistence
{
    public class CaseWorkerTaskQueryRepository : ICaseWorkerTaskQueryRepository
    {
        private static Dictionary<string, string> MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "performer", "PerformerRole" },
            { "case_plan_id", "CasePlanId" },
            { "case_plan_instance_id", "CasePlanInstanceId" },
            { "case_plan_element_instance_id", "CasePlanElementInstanceId" },
            { "type", "TaskType" },
            { "status", "Status" },
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };
        private readonly CaseManagementDbContext _caseManagementDbContext;

        public CaseWorkerTaskQueryRepository(CaseManagementDbContext caseManagementDbContext)
        {
            _caseManagementDbContext = caseManagementDbContext;
        }

        public async Task<SearchResult<CaseWorkerTaskResult>> Find(FindCaseWorkerTasksParameter parameter, CancellationToken token)
        {
            IQueryable<CaseWorkerTaskAggregate> result = _caseManagementDbContext.CaseWorkers;
            if (MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            var content = await result.ToListAsync(token);
            return new SearchResult<CaseWorkerTaskResult>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content.Select(_ => CaseWorkerTaskResult.ToDTO(_)).ToList()
            };
        }

        public async Task<CaseWorkerTaskResult> Get(string id, CancellationToken token)
        {
            var result = await _caseManagementDbContext.CaseWorkers.FirstOrDefaultAsync(_ => _.AggregateId == id, token);
            if (result == null)
            {
                return null;
            }

            return CaseWorkerTaskResult.ToDTO(result);
        }
    }
}
