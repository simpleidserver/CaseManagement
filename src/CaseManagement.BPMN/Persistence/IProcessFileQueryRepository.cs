using CaseManagement.BPMN.Persistence.Parameters;
using CaseManagement.BPMN.ProcessFile.Results;
using CaseManagement.Common.Results;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence
{
    public interface IProcessFileQueryRepository
    {
        Task<ProcessFileResult> Get(string id, CancellationToken token);
        Task<SearchResult<ProcessFileResult>> Find(FindProcessFilesParameter parameter, CancellationToken token);
    }
}
