
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCMMNWorkflowInstanceCommandRepository : ICMMNWorkflowInstanceCommandRepository
    {
        private ConcurrentBag<CMMNWorkflowInstance> _instances;

        public InMemoryCMMNWorkflowInstanceCommandRepository(ConcurrentBag<CMMNWorkflowInstance> instances)
        {
            _instances = instances;
        }

        public void Add(CMMNWorkflowInstance workflowInstance)
        {
            _instances.Add((CMMNWorkflowInstance)workflowInstance.Clone());
        }

        public void Update(CMMNWorkflowInstance workflowInstance)
        {
            var instance = _instances.First(i => i.Id == workflowInstance.Id);
            _instances.Remove(instance);
            _instances.Add((CMMNWorkflowInstance)workflowInstance.Clone());
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
