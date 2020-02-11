using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseFileQueryRepository
    {
        Task<CaseFileAggregate> FindById(string id);
        Task<FindResponse<CaseFileAggregate>> Find(FindCaseFilesParameter parameter);
        Task<int> Count();
    }
}
