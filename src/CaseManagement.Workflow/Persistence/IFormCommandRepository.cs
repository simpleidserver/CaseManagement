using CaseManagement.Workflow.Domains;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence
{
    public interface IFormCommandRepository
    {
        void Add(Form form);
        Task<int> SaveChanges();
    }
}
