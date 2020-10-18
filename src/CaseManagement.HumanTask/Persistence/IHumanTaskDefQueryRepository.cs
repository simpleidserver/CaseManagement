using CaseManagement.Common.Responses;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence.Parameters;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence
{
    public interface IHumanTaskDefQueryRepository
    {
        Task<HumanTaskDefinitionAggregate> Get(string name, CancellationToken token);
        Task<FindResponse<HumanTaskDefinitionAggregate>> Search(SearchHumanTaskDefParameter parameter, CancellationToken token);
    }
}
