using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
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

        public void Delete(CaseFileAggregate caseFile)
        {
            _caseFiles.Remove(_caseFiles.First(a => a.Id == caseFile.Id));
        }

        public void Add(CaseFileAggregate caseFile)
        {
            _caseFiles.Add((CaseFileAggregate)caseFile.Clone());
        }

        public void Update(CaseFileAggregate caseFile)
        {
            _caseFiles.Remove(_caseFiles.First(a => a.Id == caseFile.Id));
            _caseFiles.Add((CaseFileAggregate)caseFile.Clone());
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
