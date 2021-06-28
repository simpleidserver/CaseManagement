using CaseManagement.CMMN.CaseWorkerTask.Results;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Results;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseWorkerTaskQueryRepository
    {
        Task<CaseWorkerTaskResult> Get(string id, CancellationToken token);
        Task<SearchResult<CaseWorkerTaskResult>> Find(FindCaseWorkerTasksParameter parameter, CancellationToken token);
    }
}
