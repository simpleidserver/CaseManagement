using CaseManagement.CMMN.Domains;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF.Persistence
{
    public class CaseFileCommandRepository : ICaseFileCommandRepository
    {
        private readonly CaseManagementDbContext _caseManagementDbContext;

        public CaseFileCommandRepository(CaseManagementDbContext caseManagementDbContext)
        {
            _caseManagementDbContext = caseManagementDbContext;
        }

        public Task<CaseFileAggregate> Get(string id, CancellationToken token)
        {
            return _caseManagementDbContext.CaseFiles.FirstOrDefaultAsync(_ => _.AggregateId == id, token);
        }

        public Task Add(CaseFileAggregate caseFile, CancellationToken token)
        {
            _caseManagementDbContext.CaseFiles.Add(caseFile);
            return Task.CompletedTask;
        }

        public Task Delete(CaseFileAggregate caseFile, CancellationToken token)
        {
            _caseManagementDbContext.CaseFiles.Remove(caseFile);
            return Task.CompletedTask;
        }

        public Task Update(CaseFileAggregate caseFile, CancellationToken token)
        {
            _caseManagementDbContext.CaseFiles.Update(caseFile);
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _caseManagementDbContext.SaveChangesAsync(token);
        }
    }
}
