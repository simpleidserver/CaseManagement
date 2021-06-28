using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanInstanceCommandRepository
    {
        Task<CasePlanInstanceAggregate> Get(string id, CancellationToken token);
        Task Add(CasePlanInstanceAggregate workflowInstance, CancellationToken token);
        Task Update(CasePlanInstanceAggregate workflowInstance, CancellationToken token);
        Task<int> SaveChanges(CancellationToken token);
    }
}
