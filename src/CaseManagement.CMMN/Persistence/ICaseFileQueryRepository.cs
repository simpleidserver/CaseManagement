using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseFileQueryRepository
    {
        Task<CaseFileAggregate> Get(string id, CancellationToken token);
        Task<FindResponse<CaseFileAggregate>> Find(FindCaseFilesParameter parameter, CancellationToken token);
        Task<int> Count(CancellationToken token);
    }
}
