using CaseManagement.HumanTask.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.EF.Persistence
{
    public class NotificationInstanceCommandRepository : INotificationInstanceCommandRepository
    {
        private readonly HumanTaskDBContext _dbContext;

        public NotificationInstanceCommandRepository(HumanTaskDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public Task<bool> Add(NotificationInstanceAggregate notification, CancellationToken token)
        {
            _dbContext.Add(notification);
            return Task.FromResult(true);
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }
    }
}
