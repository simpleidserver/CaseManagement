using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanificationQueryRepository
    {
        Task<CasePlanificationAggregate> FindById(string caseInstanceId, string caseElementId);
        Task<FindResponse<CasePlanificationAggregate>> Find(FindCasePlanificationParameter parameter);
    }
}
