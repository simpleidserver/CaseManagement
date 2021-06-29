using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.BPMN.ProcessInstance.Results;
using CaseManagement.Common.Results;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.EF.Persistence
{
    public class ProcessInstanceQueryRepository : IProcessInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_PROCESSINSTANCE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "status", "Status" },
            { "processFileId", "ProcessFileId" },
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };
        private readonly BPMNDbContext _dbContext;

        public ProcessInstanceQueryRepository(BPMNDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProcessInstanceResult> Get(string id, CancellationToken token)
        {
            var result = await _dbContext.ProcessInstances.FirstOrDefaultAsync(_ => _.AggregateId == id, token);
            if (result == null)
            {
                return null;
            }

            return ProcessInstanceResult.ToDto(result);
        }

        public async Task<SearchResult<ProcessInstanceResult>> Find(FindProcessInstancesParameter parameter, CancellationToken token)
        {
            IQueryable<ProcessInstanceAggregate> result = _dbContext.ProcessInstances;
            if (!string.IsNullOrEmpty(parameter.ProcessFileId))
            {
                result = result.Where(_ => _.ProcessFileId == parameter.ProcessFileId);
            }

            if (parameter.Status != null)
            {
                result = result.Where(_ => _.Status == parameter.Status.Value);
            }

            if (MAPPING_PROCESSINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_PROCESSINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = await result.CountAsync(token);
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            var content = await result.ToListAsync(token);
            return new SearchResult<ProcessInstanceResult>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = content.Select(_ => ProcessInstanceResult.ToDto(_)).ToList()
            };
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
