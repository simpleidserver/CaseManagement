using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseFileQueryRepository
    {
        Task<CaseFileAggregate> Get(string id, CancellationToken canellationToken);
        Task<FindResponse<CaseFileAggregate>> Find(FindCaseFilesParameter parameter, CancellationToken cancellationToken);
        Task<int> Count(CancellationToken cancellatonToken);
    }
}
