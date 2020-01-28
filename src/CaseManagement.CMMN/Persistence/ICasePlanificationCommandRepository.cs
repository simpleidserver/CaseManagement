using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanificationCommandRepository
    {
        void Delete(CasePlanificationAggregate casePlanification);
        void Add(CasePlanificationAggregate casePlanification);
        void Update(CasePlanificationAggregate casePlanification);
        Task<int> SaveChanges();
    }
}
