using CaseManagement.CMMN.Domains;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCaseFileCommandRepository : ICaseFileCommandRepository
    {
        private readonly ConcurrentBag<CaseFileAggregate> _caseFiles;

        public InMemoryCaseFileCommandRepository(ConcurrentBag<CaseFileAggregate> caseFiles)
        {
            _caseFiles = caseFiles;
        }

        public Task Delete(CaseFileAggregate caseFile, CancellationToken token)
        {
            _caseFiles.Remove(_caseFiles.First(a => a.AggregateId == caseFile.AggregateId));
            return Task.CompletedTask;
        }

        public Task Add(CaseFileAggregate caseFile, CancellationToken token)
        {
            _caseFiles.Add((CaseFileAggregate)caseFile.Clone());
            return Task.CompletedTask;
        }

        public Task Update(CaseFileAggregate caseFile, CancellationToken token)
        {
            _caseFiles.Remove(_caseFiles.First(a => a.AggregateId == caseFile.AggregateId));
            _caseFiles.Add((CaseFileAggregate)caseFile.Clone());
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}
