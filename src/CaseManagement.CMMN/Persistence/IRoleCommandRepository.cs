using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IRoleCommandRepository
    {
        void Add(RoleAggregate role);
        void Update(RoleAggregate role);
        Task<int> SaveChanges();
    }
}
