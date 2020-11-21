using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.EF.DomainMapping;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.EF.Persistence
{
    public class ProcessFileCommandRepository : IProcessFileCommandRepository
    {
        private readonly BPMNDbContext _dbContext;

        public ProcessFileCommandRepository(BPMNDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Add(ProcessFileAggregate processFile, CancellationToken token)
        {
            _dbContext.ProcessFiles.Add(processFile.ToModel());
            return Task.CompletedTask;
        }

        public async Task Update(ProcessFileAggregate processFile, CancellationToken token)
        {
            var result = await _dbContext.ProcessFiles.FirstOrDefaultAsync(_ => _.Id == processFile.AggregateId, token);
            result.Name = processFile.Name;
            result.Description = processFile.Description;
            result.Payload = processFile.Payload;
            result.Status = processFile.Status;
            result.UpdateDateTime = processFile.UpdateDateTime;
            result.Version = processFile.Version;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }
    }
}
