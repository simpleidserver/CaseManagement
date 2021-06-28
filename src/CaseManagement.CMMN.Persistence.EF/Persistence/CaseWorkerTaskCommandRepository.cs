using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF.Persistence
{
    public class CaseWorkerTaskCommandRepository : ICaseWorkerTaskCommandRepository
    {
        private readonly CaseManagementDbContext _caseManagementDbContext;

        public CaseWorkerTaskCommandRepository(CaseManagementDbContext caseManagementDbContext)
        {
            _caseManagementDbContext = caseManagementDbContext;
        }

        public Task<CaseWorkerTaskAggregate> Get(string id, CancellationToken token)
        {
            return _caseManagementDbContext.CaseWorkers.FirstOrDefaultAsync(_ => _.AggregateId == id, token);
        }

        public Task Add(CaseWorkerTaskAggregate caseWorkerTask, CancellationToken token)
        {
            _caseManagementDbContext.CaseWorkers.Add(caseWorkerTask);
            return Task.CompletedTask;
        }

        public Task Delete(CaseWorkerTaskAggregate caseWorkerTask, CancellationToken token)
        {
            _caseManagementDbContext.CaseWorkers.Remove(caseWorkerTask);
            return Task.CompletedTask;
        }

        public Task Update(CaseWorkerTaskAggregate caseWorkerTask, CancellationToken token)
        {
            _caseManagementDbContext.CaseWorkers.Update(caseWorkerTask);
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _caseManagementDbContext.SaveChangesAsync(token);
        }
    }
}
