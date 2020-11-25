using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.InMemory
{
    public class InMemoryProcessInstanceQueryRepository : IProcessInstanceQueryRepository
    {
        private static Dictionary<string, string> MAPPING_PROCESSINSTANCE_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "status", "Status" },
            { "processFileId", "ProcessFileId" },
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };
        private ConcurrentBag<ProcessInstanceAggregate> _instances;

        public InMemoryProcessInstanceQueryRepository(ConcurrentBag<ProcessInstanceAggregate> instances)
        {
            _instances = instances;
        }

        public Task<ProcessInstanceAggregate> Get(string id, CancellationToken token)
        {
            var result = _instances.FirstOrDefault(i => i.AggregateId == id);
            if (result == null)
            {
                return Task.FromResult((ProcessInstanceAggregate)null);
            }

            return Task.FromResult(result.Clone() as ProcessInstanceAggregate);
        }

        public Task<FindResponse<ProcessInstanceAggregate>> Find(FindProcessInstancesParameter parameter, CancellationToken token)
        {
            IQueryable<ProcessInstanceAggregate> result = _instances.AsQueryable();
            if (!string.IsNullOrEmpty(parameter.ProcessFileId))
            {
                result = result.Where(_ => _.ProcessFileId == parameter.ProcessFileId);
            }

            if (parameter.Status != null)
            {
                result = result.Where(_ => _.Status == (ProcessInstanceStatus)parameter.Status.Value);
            }

            if (MAPPING_PROCESSINSTANCE_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = result.InvokeOrderBy(MAPPING_PROCESSINSTANCE_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<ProcessInstanceAggregate>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }

        public void Dispose()
        {
        }
    }
}
