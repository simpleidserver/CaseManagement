using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanCommandRepository
    {
        Task<CasePlanAggregate> Get(string id, CancellationToken token);
        Task Update(CasePlanAggregate workflowDefinition, CancellationToken token);
        Task Add(CasePlanAggregate workflowDefinition, CancellationToken token);
        Task Delete(CasePlanAggregate workflowDefinition, CancellationToken token);
        Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
