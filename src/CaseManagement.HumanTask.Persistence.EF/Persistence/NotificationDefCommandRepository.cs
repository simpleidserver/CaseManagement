using CaseManagement.HumanTask.Domains;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.EF.Persistence
{
    public class NotificationDefCommandRepository : INotificationDefCommandRepository
    {
        private readonly HumanTaskDBContext _dbContext;

        public NotificationDefCommandRepository(HumanTaskDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> Add(NotificationDefinitionAggregate notificationDef, CancellationToken token)
        {
            _dbContext.NotificationDefinitions.Add(notificationDef);
            return Task.FromResult(true);
        }

        public async Task<bool> Delete(string name, CancellationToken token)
        {
            var notificationDef = await _dbContext.NotificationDefinitions.FirstOrDefaultAsync(_ => _.Name == name, token);
            _dbContext.NotificationDefinitions.Remove(notificationDef);
            return true;
        }

        public Task<bool> Update(NotificationDefinitionAggregate notificationDef, CancellationToken token)
        {
            _dbContext.Entry(notificationDef).State = EntityState.Modified;
            return Task.FromResult(true);
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }
    }
}
