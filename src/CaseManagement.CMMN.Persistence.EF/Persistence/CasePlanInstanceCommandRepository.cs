using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF.Persistence
{
    public class CasePlanInstanceCommandRepository : ICasePlanInstanceCommandRepository
    {
        private readonly CaseManagementDbContext _dbContext;

        public CasePlanInstanceCommandRepository(CaseManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<CasePlanInstanceAggregate> Get(string id, CancellationToken token)
        {
            return _dbContext.CasePlanInstances.FirstOrDefaultAsync(_ => _.AggregateId == id, token);
        }

        public Task Add(CasePlanInstanceAggregate workflowInstance, CancellationToken token)
        {
            _dbContext.CasePlanInstances.Add(workflowInstance);
            return Task.CompletedTask;
        }

        public Task Update(CasePlanInstanceAggregate workflowInstance, CancellationToken token)
        {
            _dbContext.CasePlanInstances.Update(workflowInstance);
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
