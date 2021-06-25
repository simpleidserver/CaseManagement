using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.EF.Persistence
{
    public class ProcessInstanceCommandRepository : IProcessInstanceCommandRepository
    {
        private readonly BPMNDbContext _dbContext;

        public ProcessInstanceCommandRepository(BPMNDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ProcessInstanceAggregate> Get(string id, CancellationToken cancellationToken)
        {
            return _dbContext.ProcessInstances.FirstOrDefaultAsync(p => p.AggregateId == id, cancellationToken);
        }

        public Task Add(ProcessInstanceAggregate processInstance, CancellationToken token)
        {
            _dbContext.ProcessInstances.Add(processInstance);
            return Task.CompletedTask;
        }

        public Task Update(ProcessInstanceAggregate processInstance, CancellationToken token)
        {
            _dbContext.ProcessInstances.Update(processInstance);
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }
    }
}
