using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.EF.DomainMapping;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.EF.Persistence
{
    public class ProcessFileQueryRepository : IProcessFileQueryRepository
    {
        private readonly BPMNDbContext _dbContext;

        public ProcessFileQueryRepository(BPMNDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProcessFileAggregate> Get(string id, CancellationToken token)
        {
            var result = await _dbContext.ProcessFiles.FirstOrDefaultAsync(_ => _.Id == id);
            return result?.ToDomain();
        }
    }
}
