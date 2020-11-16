using CaseManagement.BPMN.Domains;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.InMemory
{
    public class InMemoryProcessFileQueryRepository : IProcessFileQueryRepository
    {
        private readonly ConcurrentBag<ProcessFileAggregate> _processFiles;

        public InMemoryProcessFileQueryRepository(ConcurrentBag<ProcessFileAggregate> processFiles)
        {
            _processFiles = processFiles;
        }

        public Task<ProcessFileAggregate> Get(string id, CancellationToken token)
        {
            return Task.FromResult(_processFiles.FirstOrDefault(_ => _.AggregateId == id));
        }
    }
}
