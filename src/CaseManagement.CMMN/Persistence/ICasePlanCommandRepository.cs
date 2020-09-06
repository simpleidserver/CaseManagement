using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanCommandRepository
    {
        void Update(CasePlanAggregate casePlan);
        void Add(CasePlanAggregate casePlan);
        void Delete(CasePlanAggregate casePlan);
        Task<int> SaveChanges(CancellationToken token);
    }
}
