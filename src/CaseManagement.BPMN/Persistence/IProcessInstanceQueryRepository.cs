using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence
{
    public interface IProcessInstanceQueryRepository : IDisposable
    {
        Task<ProcessInstanceAggregate> Get(string id, CancellationToken token);
        Task<FindResponse<ProcessInstanceAggregate>> Find(FindProcessInstancesParameter parameter, CancellationToken token);
    }
}
