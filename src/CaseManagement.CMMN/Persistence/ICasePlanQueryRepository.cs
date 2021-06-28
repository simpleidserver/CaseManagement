using CaseManagement.CMMN.CasePlan.Results;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Results;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanQueryRepository
    {
        Task<CasePlanResult> Get(string id, CancellationToken token);
        Task<SearchResult<CasePlanResult>> Find(FindCasePlansParameter parameter, CancellationToken token);
        Task<int> Count(CancellationToken token);
    }
}
