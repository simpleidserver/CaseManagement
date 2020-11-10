using CaseManagement.Common.Responses;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence.Parameters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence
{
    public interface IHumanTaskInstanceQueryRepository
    {
        Task<HumanTaskInstanceAggregate> Get(string id, CancellationToken token);
        Task<ICollection<HumanTaskInstanceAggregate>> GetSubTasks(string parentHumanTaskId, CancellationToken token);
        Task<ICollection<HumanTaskInstanceAggregate>> GetPendingLst(CancellationToken token);
        Task<ICollection<HumanTaskInstanceAggregate>> GetPendingDeadLines(CancellationToken token, DateTime currentDateTime);
        Task<FindResponse<HumanTaskInstanceEventHistory>> FindHumanTaskInstanceHistory(FindHumanTaskInstanceHistoryParameter parameter, CancellationToken token);
        Task<FindResponse<HumanTaskInstanceAggregate>> Search(SearchHumanTaskInstanceParameter parameter, CancellationToken token);
    }
}
