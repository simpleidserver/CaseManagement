using CaseManagement.BPMN.Domains.DelegateConfiguration;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.InMemory
{
    public class InMemoryDelegateConfigurationRepository : IDelegateConfigurationRepository
    {
        private readonly ConcurrentBag<DelegateConfigurationAggregate> _delegateConfigurationAggregates;

        public InMemoryDelegateConfigurationRepository(ConcurrentBag<DelegateConfigurationAggregate> delegateConfigurationAggregates)
        {
            _delegateConfigurationAggregates = delegateConfigurationAggregates;
        }

        public Task<DelegateConfigurationAggregate> Get(string delegateId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_delegateConfigurationAggregates.FirstOrDefault(_ => _.AggregateId == delegateId));
        }
    }
}
