using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.EF.DomainMapping;
using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.EF.Persistence
{
    public class ProcessInstanceQueryRepository : IProcessInstanceQueryRepository
    {
        private readonly BPMNDbContext _dbContext;

        public ProcessInstanceQueryRepository(BPMNDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<FindResponse<ProcessInstanceAggregate>> Find(FindProcessInstancesParameter parameter, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<ProcessInstanceAggregate> Get(string id, CancellationToken token)
        {
            var result = await _dbContext.ProcessInstances.FirstOrDefaultAsync(_ => _.AggregateId == id, token);
            if (result == null)
            {
                return null;
            }

            return result.ToDomain();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
