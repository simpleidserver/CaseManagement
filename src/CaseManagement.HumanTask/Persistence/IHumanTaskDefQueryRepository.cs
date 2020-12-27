using CaseManagement.Common.Responses;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Persistence.Parameters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence
{
    public interface IHumanTaskDefQueryRepository
    {
        Task<HumanTaskDefinitionAggregate> Get(string id, CancellationToken token);
        Task<ICollection<HumanTaskDefinitionAggregate>> GetAll(CancellationToken token);
        Task<HumanTaskDefinitionAggregate> GetLatest(string name, CancellationToken token);
        Task<FindResponse<HumanTaskDefinitionAggregate>> Search(SearchHumanTaskDefParameter parameter, CancellationToken token);
    }
}
