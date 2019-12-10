using CaseManagement.Workflow.Domains;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence
{
    public interface IRoleCommandRepository
    {
        void Add(RoleAggregate role);
        void Update(RoleAggregate role);
        Task<int> SaveChanges();
    }
}
