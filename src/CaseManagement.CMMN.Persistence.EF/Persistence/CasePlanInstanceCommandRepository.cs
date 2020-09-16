using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.EF.DomainMapping;
using CaseManagement.CMMN.Persistence.EF.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF.Persistence
{
    public class CasePlanInstanceCommandRepository : ICasePlanInstanceCommandRepository
    {
        private readonly CaseManagementDbContext _dbContext;

        public CasePlanInstanceCommandRepository(CaseManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Add(CasePlanInstanceAggregate workflowInstance, CancellationToken token)
        {
            var record = workflowInstance.ToModel();
            _dbContext.CasePlanInstances.Add(record);
            return Task.CompletedTask;
        }

        public async Task Update(CasePlanInstanceAggregate workflowInstance, CancellationToken token)
        {
            using (var lck = await _dbContext.Lock())
            {
                var record = await _dbContext.CasePlanInstances
                    .Include(_ => _.Roles).ThenInclude(_ => _.Claims)
                    .Include(_ => _.Files)
                    .Include(_ => _.WorkerTasks)
                    .Include(_ => _.Children).ThenInclude(_ => _.Children)
                    .FirstOrDefaultAsync(_ => _.Id == workflowInstance.AggregateId, token);
                if (record == null)
                {
                    return;
                }

                _dbContext.Roles.RemoveRange(record.Roles);
                _dbContext.CasePlanInstanceWorkerTaskLst.RemoveRange(record.WorkerTasks);
                _dbContext.CasePlanElementInstanceLst.RemoveRange(record.Children);
                _dbContext.CasePlanInstanceFileItemLst.RemoveRange(record.Files);
                record.CaseState = (int?)workflowInstance.State;
                record.Version = workflowInstance.Version;
                record.Name = workflowInstance.Name;
                record.ExecutionContext = workflowInstance.ExecutionContext == null ? null : JsonConvert.SerializeObject(workflowInstance.ExecutionContext);
                record.Roles = workflowInstance.Roles.Select(_ => _.ToModel(workflowInstance.AggregateId, workflowInstance.CaseOwner)).ToList();
                record.Files = workflowInstance.Files.Select(_ => _.ToModel(workflowInstance.AggregateId)).ToList();
                record.WorkerTasks = workflowInstance.WorkerTasks.Select(_ => _.ToModel(workflowInstance.AggregateId)).ToList();
                record.Children = workflowInstance.Children == null ? new List<CasePlanElementInstanceModel>() : workflowInstance.Children.Select(_ => _.ToModel(workflowInstance.AggregateId)).ToList();
                record.UpdateDateTime = workflowInstance.UpdateDateTime;
            }
        }

        public async Task<int> SaveChanges(CancellationToken token)
        {
            using (var lck = await _dbContext.Lock())
            {
                return await _dbContext.SaveChangesAsync();
            }
        }
    }
}
