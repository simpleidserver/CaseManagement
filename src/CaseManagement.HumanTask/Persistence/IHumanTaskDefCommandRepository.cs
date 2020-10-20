using CaseManagement.HumanTask.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence
{
    public interface IHumanTaskDefCommandRepository
    {
        Task<bool> Add(HumanTaskDefinitionAggregate humanTaskDef, CancellationToken token);
        Task<bool> Delete(string name, CancellationToken token);
        Task<bool> Update(HumanTaskDefinitionAggregate name, CancellationToken token);
        Task<int> SaveChanges(CancellationToken token);
    }
}
