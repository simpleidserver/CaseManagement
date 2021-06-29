using CaseManagement.BPMN.Domains;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.EF.Persistence
{
    public class DelegateConfigurationRepository : IDelegateConfigurationRepository
    {
        private readonly BPMNDbContext _dbContext;

        public DelegateConfigurationRepository(BPMNDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<DelegateConfigurationAggregate> Get(string delegateId, CancellationToken cancellationToken)
        {
            return _dbContext.DelegateConfigurations.FirstOrDefaultAsync(d => d.AggregateId == delegateId, cancellationToken);
        }
    }
}
