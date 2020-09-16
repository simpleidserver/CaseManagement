using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.EF.DomainMapping;
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

        public Task Add(CaseFileAggregate caseFile, CancellationToken token)
        {
            _caseManagementDbContext.CaseFiles.Add(caseFile.ToModel());
            return Task.CompletedTask;
        }

        public async Task Delete(CaseFileAggregate caseFile, CancellationToken token)
        {
            using (var lck = await _caseManagementDbContext.Lock())
            {
                var record = await _caseManagementDbContext.CaseFiles.FirstOrDefaultAsync(_ => _.Id == caseFile.AggregateId, token);
                if (record == null)
                {
                    return;
                }

                _caseManagementDbContext.CaseFiles.Remove(record);
            }
        }

        public async Task Update(CaseFileAggregate caseFile, CancellationToken token)
        {
            using (var lck = await _caseManagementDbContext.Lock())
            {
                var record = await _caseManagementDbContext.CaseFiles.FirstOrDefaultAsync(_ => _.Id == caseFile.AggregateId, token);
                if (record == null)
                {
                    return;
                }

                record.Name = caseFile.Name;
                record.Description = caseFile.Description;
                record.UpdateDateTime = caseFile.UpdateDateTime;
                record.SerializedContent = caseFile.Payload;
                record.Owner = caseFile.Owner;
                record.Status = (int)caseFile.Status;
                record.Version = caseFile.Version;
            }
        }

        public async Task<int> SaveChanges(CancellationToken token)
        {
            using (var lck = await _caseManagementDbContext.Lock())
            {
                return await _caseManagementDbContext.SaveChangesAsync(token);
            }
        }
    }
}
