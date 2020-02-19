using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCaseWorkerTaskQueryRepository : ICaseWorkerTaskQueryRepository
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

        private ConcurrentBag<CaseWorkerTaskAggregate> _caseWorkerTaskLst;

        public InMemoryCaseWorkerTaskQueryRepository(ConcurrentBag<CaseWorkerTaskAggregate> caseWorkerTaskLst)
        {
            _caseWorkerTaskLst = caseWorkerTaskLst;
        }

        public Task<CaseWorkerTaskAggregate> FindById(string id)
        {
            return Task.FromResult(_caseWorkerTaskLst.FirstOrDefault(a => a.Id == id));
        }

        public Task<FindResponse<CaseWorkerTaskAggregate>> Find(FindCaseWorkerTasksParameter parameter)
        {
            IQueryable<CaseWorkerTaskAggregate> result = _caseWorkerTaskLst.AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameter.CasePlanId))
            {
                result = result.Where(r => r.CasePlanId == parameter.CasePlanId);
            }

            if (MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<CaseWorkerTaskAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }
    }
}
