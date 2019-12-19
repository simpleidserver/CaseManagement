using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IFormInstanceQueryRepository
    {
        Task<FormInstanceAggregate> FindById(string id);
        Task<FindResponse<FormInstanceAggregate>> Find(FindFormInstanceParameter parameter);
    }
}
