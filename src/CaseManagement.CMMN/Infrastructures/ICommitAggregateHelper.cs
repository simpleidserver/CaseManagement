using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public interface ICommitAggregateHelper
    {
        Task Commit<T>(T aggregate, string streamName, string queueName) where T : BaseAggregate;
        Task Commit<T>(T aggregate, ICollection<DomainEvent> evts, int aggregateVersion, string streamName, string queueName) where T : BaseAggregate;
    }
}
