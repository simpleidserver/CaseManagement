using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IFormQueryRepository
    {
        Task<FormAggregate> FindFormById(string id);
        Task<FindResponse<FormAggregate>> Find(FindFormParameter parameter);
        Task<FormAggregate> FindLatestVersion(string formId);
    }
}
