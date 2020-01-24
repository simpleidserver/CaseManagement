using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCaseInstanceCommandRepository : ICaseInstanceCommandRepository
    {
        private ConcurrentBag<Domains.CaseInstance> _instances;

        public InMemoryCaseInstanceCommandRepository(ConcurrentBag<Domains.CaseInstance> instances)
        {
            _instances = instances;
        }

        public void Add(Domains.CaseInstance workflowInstance)
        {
            _instances.Add((Domains.CaseInstance)workflowInstance.Clone());
        }

        public void Update(Domains.CaseInstance workflowInstance)
        {
            var instance = _instances.First(i => i.Id == workflowInstance.Id);
            _instances.Remove(instance);
            _instances.Add((Domains.CaseInstance)workflowInstance.Clone());
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
