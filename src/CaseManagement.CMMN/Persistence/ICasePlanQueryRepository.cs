using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanQueryRepository
    {
        Task<CasePlanAggregate> Get(string id, CancellationToken token);
        Task<FindResponse<CasePlanAggregate>> Find(FindCasePlansParameter parameter, CancellationToken token);
        Task<int> Count(CancellationToken token);
    }
}
