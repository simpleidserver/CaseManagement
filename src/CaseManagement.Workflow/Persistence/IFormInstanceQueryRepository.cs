using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Persistence.Parameters;
using CaseManagement.Workflow.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence
{
    public interface IFormInstanceQueryRepository
    {
        Task<FormInstanceAggregate> FindById(string id);
        Task<FindResponse<FormInstanceAggregate>> Find(FindFormInstanceParameter parameter);
    }
}
