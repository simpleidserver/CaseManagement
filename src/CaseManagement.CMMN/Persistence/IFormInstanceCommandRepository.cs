using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IFormInstanceCommandRepository
    {
        void Add(FormInstanceAggregate formInstance);
        void Update(FormInstanceAggregate formInstance);
        Task<int> SaveChanges();
    }
}
