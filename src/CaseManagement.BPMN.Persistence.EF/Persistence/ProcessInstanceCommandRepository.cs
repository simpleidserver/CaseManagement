using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.EF.DomainMapping;
using CaseManagement.BPMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
            var ets = _dbContext.ChangeTracker.Entries<ProcessInstanceModel>();
            return Task.CompletedTask;
        }

        public async Task Update(ProcessInstanceAggregate processInstance, CancellationToken token)
        {
            var result = await _dbContext.ProcessInstances
                .Include(_ => _.ItemDefs)
                .Include(_ => _.Interfaces).ThenInclude(_ => _.Operations)
                .Include(_ => _.Messages)
                .Include(_ => _.ElementDefs)
                .Include(_ => _.SequenceFlows)
                .Include(_ => _.ExecutionPathLst).ThenInclude(_ => _.Pointers).ThenInclude(_ => _.Tokens)
                .Include(_ => _.StateTransitions)
                .Include(_ => _.ElementInstances).ThenInclude(_ => _.ActivityStates)
                .FirstOrDefaultAsync(_ => _.AggregateId == processInstance.AggregateId, token);
            var rec = processInstance.ToModel(); 
            result.Status = rec.Status;
            result.NameIdentifier = rec.NameIdentifier;
            result.Version = rec.Version;
            result.UpdateDateTime = rec.UpdateDateTime;
            result.ItemDefs.Clear();
            result.Interfaces.Clear();
            result.Messages.Clear();
            result.ElementDefs.Clear();
            result.SequenceFlows.Clear();
            result.ElementInstances.Clear();
            result.ExecutionPathLst.Clear();
            result.StateTransitions.Clear();
            foreach (var itemDef in rec.ItemDefs)
            {
                result.ItemDefs.Add(itemDef);
            }

            foreach (var inter in rec.Interfaces)
            {
                result.Interfaces.Add(inter);
            }

            foreach(var msg in rec.Messages)
            {
                result.Messages.Add(msg);
            }

            foreach (var eltDef in rec.ElementDefs)
            {
                result.ElementDefs.Add(eltDef);
            }

            foreach(var sequenceFlow in rec.SequenceFlows)
            {
                result.SequenceFlows.Add(sequenceFlow);
            }

            foreach(var eltInstance in rec.ElementInstances)
            {
                result.ElementInstances.Add(eltInstance);
            }

            foreach (var executionPath in rec.ExecutionPathLst)
            {
                result.ExecutionPathLst.Add(executionPath);
            }

            foreach (var stateTransition in rec.StateTransitions)
            {
                result.StateTransitions.Add(stateTransition);
            }
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }
    }
}
