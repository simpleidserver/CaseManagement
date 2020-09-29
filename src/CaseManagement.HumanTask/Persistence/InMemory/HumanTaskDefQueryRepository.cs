using CaseManagement.HumanTask.Domains;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.InMemory
{
    public class HumanTaskDefQueryRepository : IHumanTaskDefQueryRepository
    {
        private readonly ConcurrentBag<HumanTaskDefinitionAggregate> _humanTaskDefs;

        public HumanTaskDefQueryRepository(ConcurrentBag<HumanTaskDefinitionAggregate> humanTaskDefs)
        {
            _humanTaskDefs = humanTaskDefs;
        }

        public Task<HumanTaskDefinitionAggregate> Get(string name, CancellationToken token)
        {
            return Task.FromResult((HumanTaskDefinitionAggregate)_humanTaskDefs.FirstOrDefault(_ => _.Name == name)?.Clone());
        }
    }
}
