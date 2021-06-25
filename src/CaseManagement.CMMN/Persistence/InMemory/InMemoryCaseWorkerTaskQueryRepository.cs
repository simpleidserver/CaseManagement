using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public Task<CaseWorkerTaskAggregate> Get(string id, CancellationToken token)
        {
            return Task.FromResult(_caseWorkerTaskLst.FirstOrDefault(a => a.AggregateId == id));
        }

        public Task<SearchResult<CaseWorkerTaskAggregate>> Find(FindCaseWorkerTasksParameter parameter, CancellationToken token)
        {
            IEnumerable<CaseWorkerTaskAggregate> result = _caseWorkerTaskLst.ToList();
            if (MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.AsQueryable().InvokeOrderBy(MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order).ToList();
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new SearchResult<CaseWorkerTaskAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }
    }
}
