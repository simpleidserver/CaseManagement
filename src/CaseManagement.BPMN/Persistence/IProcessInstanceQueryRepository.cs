using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.BPMN.ProcessInstance.Results;
using CaseManagement.Common.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence
{
    public interface IProcessInstanceQueryRepository : IDisposable
    {
        Task<ProcessInstanceResult> Get(string id, CancellationToken token);
        Task<SearchResult<ProcessInstanceResult>> Find(FindProcessInstancesParameter parameter, CancellationToken token);
    }
}
