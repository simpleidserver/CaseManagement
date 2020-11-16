using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.EF.DomainMapping;
using System;
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

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }
    }
}
