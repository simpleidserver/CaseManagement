using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICMMNActivationCommandRepository
    {
        void Add(CaseActivationAggregate activation);
        void Update(CaseActivationAggregate activation);
        Task<int> SaveChanges();
    }
}
