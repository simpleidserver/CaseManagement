using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseWorkerTaskCommandRepository
    {
        void Delete(CaseWorkerTaskAggregate caseWorker);
        void Add(CaseWorkerTaskAggregate caseWorker);
        void Update(CaseWorkerTaskAggregate caseWorker);
        Task<int> SaveChanges();
    }
}
