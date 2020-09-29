using CaseManagement.HumanTask.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence
{
    public interface IHumanTaskInstanceCommandRepository
    {
        Task<bool> Add(HumanTaskInstanceAggregate humanTaskInstance, CancellationToken token);
        Task<bool> Update(HumanTaskInstanceAggregate humanTaskInstance, CancellationToken token);
        Task<int> SaveChanges(CancellationToken token);
    }
}
