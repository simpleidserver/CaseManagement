using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.EF.DomainMapping;
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

        public Task Add(CasePlanAggregate workflowDefinition, CancellationToken token)
        {
            _dbContext.CasePlans.Add(workflowDefinition.ToModel());
            return Task.CompletedTask;
        }

        public async Task Delete(CasePlanAggregate workflowDefinition, CancellationToken token)
        {
            using (var lck = await _dbContext.Lock())
            {
                var result = await _dbContext.CasePlans.FirstOrDefaultAsync(_ => _.Id == workflowDefinition.AggregateId, token);
                if (result == null)
                {
                    return;
                }

                _dbContext.CasePlans.Remove(result);
            }
        }

        public async Task Update(CasePlanAggregate workflowDefinition, CancellationToken token)
        {
            using (var lck = await _dbContext.Lock())
            {
                var result = await _dbContext.CasePlans.FirstOrDefaultAsync(_ => _.Id == workflowDefinition.AggregateId, token);
                if (result == null)
                {
                    return;
                }

                _dbContext.CasePlans.Remove(result);
                _dbContext.CasePlans.Add(workflowDefinition.ToModel());
            }
        }

        public async Task<int> SaveChanges(CancellationToken token)
        {
            using (var lck = await _dbContext.Lock())
            {
                return await _dbContext.SaveChangesAsync(token);
            }
        }
    }
}
