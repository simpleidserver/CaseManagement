using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseWorkerTaskCommandRepository
    {
        Task Delete(CaseWorkerTaskAggregate caseWorker, CancellationToken token);
        Task Add(CaseWorkerTaskAggregate caseWorker, CancellationToken token);
        Task Update(CaseWorkerTaskAggregate caseWorker, CancellationToken token);
        Task<int> SaveChanges(CancellationToken token);
    }
}
