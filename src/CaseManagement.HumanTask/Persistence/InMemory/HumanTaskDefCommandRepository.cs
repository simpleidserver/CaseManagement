using CaseManagement.HumanTask.Domains;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Persistence.InMemory
{
    public class HumanTaskDefCommandRepository : IHumanTaskDefCommandRepository
    {
        private readonly ConcurrentBag<HumanTaskDefinitionAggregate> _humanTaskDefs;

        public HumanTaskDefCommandRepository(ConcurrentBag<HumanTaskDefinitionAggregate> humanTaskDefs)
        {
            _humanTaskDefs = humanTaskDefs;
        }

        public Task<bool> Add(HumanTaskDefinitionAggregate humanTaskDef, CancellationToken token)
        {
            _humanTaskDefs.Add(humanTaskDef);
            return Task.FromResult(true);
        }

        public Task<bool> Update(HumanTaskDefinitionAggregate humanTaskDef, CancellationToken token)
        {
            var r = _humanTaskDefs.First(_ => _.AggregateId == humanTaskDef.AggregateId);
            _humanTaskDefs.Remove(r);
            _humanTaskDefs.Add((HumanTaskDefinitionAggregate)humanTaskDef.Clone());
            return Task.FromResult(true);
        }

        public Task<bool> Delete(string name, CancellationToken token)
        {
            var record = _humanTaskDefs.FirstOrDefault(_ => _.Name == name);
            if (record == null)
            {
                return Task.FromResult(false);
            }

            _humanTaskDefs.Remove(record);
            return Task.FromResult(true);
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}
