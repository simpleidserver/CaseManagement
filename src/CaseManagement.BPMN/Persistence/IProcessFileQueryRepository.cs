using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence
{
    public interface IProcessFileQueryRepository
    {
        Task<ProcessFileAggregate> Get(string id, CancellationToken token);
        Task<FindResponse<ProcessFileAggregate>> Find(FindProcessFilesParameter parameter, CancellationToken token);
    }
}
