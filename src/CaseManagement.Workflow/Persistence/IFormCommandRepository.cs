using CaseManagement.Workflow.Domains;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence
{
    public interface IFormCommandRepository
    {
        void Add(FormAggregate form);
        Task<int> SaveChanges();
    }
}
