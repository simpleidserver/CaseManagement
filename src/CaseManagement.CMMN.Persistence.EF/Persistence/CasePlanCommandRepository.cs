using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF.Persistence
{
    public class CasePlanCommandRepository : ICasePlanCommandRepository
    {
        private readonly CaseManagementDbContext _dbContext;

        public CasePlanCommandRepository(CaseManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<CasePlanAggregate> Get(string id, CancellationToken token)
        {
            return _dbContext.CasePlans.FirstOrDefaultAsync(_ => _.AggregateId == id, token);
        }

        public Task Add(CasePlanAggregate casePlan, CancellationToken token)
        {
            _dbContext.CasePlans.Add(casePlan);
            return Task.CompletedTask;
        }

        public Task Delete(CasePlanAggregate casePlan, CancellationToken token)
        {
            _dbContext.CasePlans.Remove(casePlan);
            return Task.CompletedTask;
        }

        public Task Update(CasePlanAggregate casePlan, CancellationToken token)
        {
            _dbContext.CasePlans.Update(casePlan);
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }
    }
}
