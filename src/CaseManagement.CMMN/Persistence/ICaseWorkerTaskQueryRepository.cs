using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseWorkerTaskQueryRepository
    {
        Task<CaseWorkerTaskAggregate> Get(string id, CancellationToken token);
        Task<SearchResult<CaseWorkerTaskAggregate>> Find(FindCaseWorkerTasksParameter parameter, CancellationToken token);
    }
}
