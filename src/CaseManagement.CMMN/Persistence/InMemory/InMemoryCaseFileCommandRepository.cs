using CaseManagement.CMMN.Domains.CaseFile;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCaseFileCommandRepository : ICaseFileCommandRepository
    {
        private readonly ConcurrentBag<CaseFileDefinitionAggregate> _caseFiles;

        public InMemoryCaseFileCommandRepository(ConcurrentBag<CaseFileDefinitionAggregate> caseFiles)
        {
            _caseFiles = caseFiles;
        }

        public void Delete(CaseFileDefinitionAggregate caseFile)
        {
            _caseFiles.Remove(_caseFiles.First(a => a.Id == caseFile.Id));
        }

        public void Add(CaseFileDefinitionAggregate caseFile)
        {
            _caseFiles.Add((CaseFileDefinitionAggregate)caseFile.Clone());
        }

        public void Update(CaseFileDefinitionAggregate caseFile)
        {
            _caseFiles.Remove(_caseFiles.First(a => a.Id == caseFile.Id));
            _caseFiles.Add((CaseFileDefinitionAggregate)caseFile.Clone());
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
