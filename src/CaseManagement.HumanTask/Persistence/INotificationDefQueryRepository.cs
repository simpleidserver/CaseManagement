using CaseManagement.Common.Responses;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence.Parameters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence
{
    public interface INotificationDefQueryRepository
    {
        Task<NotificationDefinitionAggregate> Get(string id, CancellationToken token);
        Task<ICollection<NotificationDefinitionAggregate>> GetAll(CancellationToken token);
        Task<NotificationDefinitionAggregate> GetLatest(string name, CancellationToken token);
        Task<SearchResult<NotificationDefinitionAggregate>> Search(SearchNotificationDefParameter parameter, CancellationToken token);
    }
}
