using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.InMemory
{
    public class InMemoryProcessInstanceQueryRepository : IProcessInstanceQueryRepository
    {
        private ConcurrentBag<ProcessInstanceAggregate> _instances;

        public InMemoryProcessInstanceQueryRepository(ConcurrentBag<ProcessInstanceAggregate> instances)
        {
            _instances = instances;
        }

        public Task<FindResponse<ProcessInstanceAggregate>> Find(FindProcessInstancesParameter parameter, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<ProcessInstanceAggregate> Get(string id, CancellationToken token)
        {
            var result = _instances.FirstOrDefault(i => i.AggregateId == id);
            if (result == null)
            {
                return Task.FromResult((ProcessInstanceAggregate)null);
            }

            return Task.FromResult(result.Clone() as ProcessInstanceAggregate);
        }

        public void Dispose()
        {
        }
    }
}
