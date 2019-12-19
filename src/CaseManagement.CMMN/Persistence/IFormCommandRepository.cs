using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IFormCommandRepository
    {
        void Add(FormAggregate form);
        Task<int> SaveChanges();
    }
}
