using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.EF.DomainMapping;
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

        public Task Add(ProcessInstanceAggregate processInstance, CancellationToken token)
        {
            _dbContext.ProcessInstances.Add(processInstance.ToModel());
            return Task.CompletedTask;
        }

        public async Task Update(ProcessInstanceAggregate processInstance, CancellationToken token)
        {
            var result = await _dbContext.ProcessInstances.FirstOrDefaultAsync(_ => _.AggregateId == processInstance.AggregateId, token);
            var rec = processInstance.ToModel();
            result.ElementDefs = rec.ElementDefs;
            result.ElementInstances = rec.ElementInstances;
            result.ExecutionPathLst = rec.ExecutionPathLst;
            result.InstanceId = rec.InstanceId;
            result.Interfaces = rec.Interfaces;
            result.ItemDefs = rec.ItemDefs;
            result.Messages = rec.Messages;
            result.SequenceFlows = rec.SequenceFlows;
            result.StateTransitions = rec.StateTransitions;
            result.UpdateDateTime = rec.UpdateDateTime;
            result.Version = rec.Version;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }
    }
}
