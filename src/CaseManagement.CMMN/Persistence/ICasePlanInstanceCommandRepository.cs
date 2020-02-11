using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanInstanceCommandRepository
    {
        void Add(CasePlanInstanceAggregate workflowInstance);
        void Update(CasePlanInstanceAggregate workflowInstance);
        Task<int> SaveChanges();
    }
}