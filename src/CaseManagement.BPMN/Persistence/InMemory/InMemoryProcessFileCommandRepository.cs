using CaseManagement.BPMN.Domains;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.InMemory
{
    public class InMemoryProcessFileCommandRepository : IProcessFileCommandRepository
    {
        private readonly ConcurrentBag<ProcessFileAggregate> _processFiles;

        public InMemoryProcessFileCommandRepository(ConcurrentBag<ProcessFileAggregate> processFiles)
        {
            _processFiles = processFiles;
        }

        public Task<ProcessFileAggregate> Get(string id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_processFiles.FirstOrDefault(_ => _.AggregateId == id));
        }

        public Task Add(ProcessFileAggregate processFile, CancellationToken token)
        {
            _processFiles.Add((ProcessFileAggregate)processFile.Clone());
            return Task.CompletedTask;
        }

        public Task Update(ProcessFileAggregate processFile, CancellationToken token)
        {
            var record = _processFiles.First(_ => _.AggregateId == processFile.AggregateId);
            _processFiles.Remove(record);
            _processFiles.Add((ProcessFileAggregate)processFile.Clone());
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}
