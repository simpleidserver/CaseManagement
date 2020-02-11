using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanCommandRepository
    {
        void Update(CasePlanAggregate workflowDefinition);
        void Add(CasePlanAggregate workflowDefinition);
        void Delete(CasePlanAggregate workflowDefinition);
        Task<int> SaveChanges();
    }
}
