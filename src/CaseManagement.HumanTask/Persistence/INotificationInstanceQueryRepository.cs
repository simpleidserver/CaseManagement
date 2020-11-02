using CaseManagement.Common.Responses;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence.Parameters;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence
{
    public interface INotificationInstanceQueryRepository
    {
        Task<NotificationInstanceAggregate> Get(string id, CancellationToken token);
        Task<FindResponse<NotificationInstanceAggregate>> Find(FindNotificationInstanceParameter parameter, CancellationToken token);
    }
}
