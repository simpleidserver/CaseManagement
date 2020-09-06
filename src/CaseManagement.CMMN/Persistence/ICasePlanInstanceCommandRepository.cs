using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICasePlanInstanceCommandRepository
    {
        void Add(CasePlanInstanceAggregate casePlanInstance);
        void Update(CasePlanInstanceAggregate casePlanInstance);
        Task<int> SaveChanges(CancellationToken token);
    }
}