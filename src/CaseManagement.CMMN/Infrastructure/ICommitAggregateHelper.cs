using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure
{
    public interface ICommitAggregateHelper
    {
        Task Commit<T>(T aggregate, string streamName, CancellationToken cancellationToken) where T : BaseAggregate;
        Task Commit<T>(T aggregate, ICollection<DomainEvent> evts, int aggregateVersion, string streamName, CancellationToken token) where T : BaseAggregate;
    }
}
