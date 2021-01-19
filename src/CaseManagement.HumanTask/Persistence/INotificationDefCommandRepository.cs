using CaseManagement.HumanTask.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence
{
    public interface INotificationDefCommandRepository
    {
        Task<bool> Add(NotificationDefinitionAggregate notificationDef, CancellationToken token);
        Task<bool> Update(NotificationDefinitionAggregate notificationDef, CancellationToken token);
        Task<bool> Delete(string name, CancellationToken token);
        Task<int> SaveChanges(CancellationToken token);
    }
}
