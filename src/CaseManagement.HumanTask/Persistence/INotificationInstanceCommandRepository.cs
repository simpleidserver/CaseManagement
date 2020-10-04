using CaseManagement.HumanTask.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence
{
    public interface INotificationInstanceCommandRepository
    {
        Task<bool> Add(NotificationInstanceAggregate notification, CancellationToken token);
        Task<int> SaveChanges(CancellationToken token);
    }
}
