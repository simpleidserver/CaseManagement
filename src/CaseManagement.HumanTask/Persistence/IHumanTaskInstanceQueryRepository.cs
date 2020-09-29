using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence
{
    public interface IHumanTaskInstanceQueryRepository
    {
        Task<HumanTaskInstanceAggregate> Get(string id, CancellationToken token);
        Task<ICollection<HumanTaskInstanceAggregate>> GetPendingLst(CancellationToken token);
    }
}
