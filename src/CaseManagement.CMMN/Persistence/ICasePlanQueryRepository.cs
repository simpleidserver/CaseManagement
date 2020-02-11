using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanQueryRepository
    {
        Task<CasePlanAggregate> FindById(string id);
        Task<FindResponse<CasePlanAggregate>> Find(FindCasePlansParameter parameter);
        Task<int> Count();
    }
}
