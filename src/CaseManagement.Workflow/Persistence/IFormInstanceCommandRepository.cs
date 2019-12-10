using CaseManagement.Workflow.Domains;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence
{
    public interface IFormInstanceCommandRepository
    {
        void Add(FormInstanceAggregate formInstance);
        void Update(FormInstanceAggregate formInstance);
        Task<int> SaveChanges();
    }
}
