using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanInstanceQueryRepository
    {
        Task<CasePlanInstanceAggregate> Get(string id);
        Task<FindResponse<CasePlanInstanceAggregate>> Find(FindWorkflowInstanceParameter parameter);
    }
}
