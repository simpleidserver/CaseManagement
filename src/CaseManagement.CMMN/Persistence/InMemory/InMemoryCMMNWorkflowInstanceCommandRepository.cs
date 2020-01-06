
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCMMNWorkflowInstanceCommandRepository : ICMMNWorkflowInstanceCommandRepository
    {
        private ICollection<CMMNWorkflowInstance> _instances;

        public InMemoryCMMNWorkflowInstanceCommandRepository(ICollection<CMMNWorkflowInstance> instances)
        {
            _instances = instances;
        }
        
        public void Add(CMMNWorkflowInstance workflowInstance)
        {
            lock(_instances)
            {
                _instances.Add((CMMNWorkflowInstance)workflowInstance.Clone());
            }
        }

        public void Update(CMMNWorkflowInstance workflowInstance)
        {
            lock(_instances)
            {
                var instance = _instances.First(i => i.Id == workflowInstance.Id);
                _instances.Remove(instance);
                _instances.Add((CMMNWorkflowInstance)workflowInstance.Clone());
            }
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
