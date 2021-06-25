using CaseManagement.BPMN.Domains;
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

        public Task<ProcessFileAggregate> Get(string id, CancellationToken cancellationToken)
        {
            return _dbContext.ProcessFiles.FirstOrDefaultAsync(p => p.AggregateId == id, cancellationToken);
        }

        public Task Add(ProcessFileAggregate processFile, CancellationToken token)
        {
            _dbContext.ProcessFiles.Add(processFile);
            return Task.CompletedTask;
        }

        public Task Update(ProcessFileAggregate processFile, CancellationToken token)
        {
            _dbContext.ProcessFiles.Update(processFile);
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }
    }
}
