using CaseManagement.CMMN.CaseFile.Results;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Results;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseFileQueryRepository
    {
        Task<CaseFileResult> Get(string id, CancellationToken token);
        Task<SearchResult<CaseFileResult>> Find(FindCaseFilesParameter parameter, CancellationToken token);
        Task<int> Count(CancellationToken token);
    }
}
