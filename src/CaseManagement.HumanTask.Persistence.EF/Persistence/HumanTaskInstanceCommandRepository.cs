using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.EF.Persistence
{
    public class HumanTaskInstanceCommandRepository : IHumanTaskInstanceCommandRepository
    {
        private HumanTaskDBContext _dbContext;

        public HumanTaskInstanceCommandRepository(HumanTaskDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> Add(HumanTaskInstanceAggregate humanTaskInstance, CancellationToken token)
        {
            _dbContext.HumanTaskInstanceAggregate.Add(humanTaskInstance);
            return Task.FromResult(true);
        }

        public Task<bool> Update(HumanTaskInstanceAggregate humanTaskInstance, CancellationToken token)
        {
            _dbContext.Entry(humanTaskInstance).State = EntityState.Modified;
            return Task.FromResult(true);
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }
    }
}
