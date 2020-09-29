using CaseManagement.HumanTask.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence
{
    public interface IHumanTaskDefQueryRepository
    {
        Task<HumanTaskDefinitionAggregate> Get(string name, CancellationToken token);
    }
}
