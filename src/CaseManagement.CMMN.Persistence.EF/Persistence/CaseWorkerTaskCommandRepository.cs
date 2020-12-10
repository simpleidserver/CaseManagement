using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.EF.DomainMapping;
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

        public Task Add(CaseWorkerTaskAggregate caseWorkerTask, CancellationToken token)
        {
            var record = caseWorkerTask.ToModel();
            _caseManagementDbContext.CaseWorkers.Add(record);
            return Task.CompletedTask;
        }

        public async Task Delete(CaseWorkerTaskAggregate caseWorkerTask, CancellationToken token)
        {
            using (var lck = await _caseManagementDbContext.Lock())
            {
                var record = await _caseManagementDbContext.CaseWorkers.FirstOrDefaultAsync(_ => _.Id == caseWorkerTask.AggregateId, token);
                if (record == null)
                {
                    return;
                }

                _caseManagementDbContext.CaseWorkers.Remove(record);
            }
        }

        public async Task Update(CaseWorkerTaskAggregate caseWorkerTask, CancellationToken token)
        {
            using (var lck = await _caseManagementDbContext.Lock())
            {
                var record = await _caseManagementDbContext.CaseWorkers.FirstOrDefaultAsync(_ => _.Id == caseWorkerTask.AggregateId, token);
                if (record == null)
                {
                    return;
                }

                _caseManagementDbContext.CaseWorkers.Remove(record);
                _caseManagementDbContext.CaseWorkers.Add(caseWorkerTask.ToModel());
            }
        }

        public async Task<int> SaveChanges(CancellationToken token)
        {
            using (var lck = await _caseManagementDbContext.Lock())
            {
                return await _caseManagementDbContext.SaveChangesAsync(token);
            }
        }
    }
}
