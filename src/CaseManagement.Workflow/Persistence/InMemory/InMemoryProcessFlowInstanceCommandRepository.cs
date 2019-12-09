using CaseManagement.Workflow.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence.InMemory
{
    public class InMemoryProcessFlowInstanceCommandRepository : IProcessFlowInstanceCommandRepository
    {
        private static object _obj = new object();
        private readonly ICollection<ProcessFlowInstance> _processFlowInstances;

        public InMemoryProcessFlowInstanceCommandRepository(ICollection<ProcessFlowInstance> processFlowInstances)
        {
            _processFlowInstances = processFlowInstances;
        }

        public void Add(ProcessFlowInstance processFlowInstance)
        {
            lock(_processFlowInstances)
            {
                _processFlowInstances.Add((ProcessFlowInstance)processFlowInstance.Clone());
            }
        }

        public void Update(ProcessFlowInstance processFlowInstance)
        {
            lock(_processFlowInstances)
            {
                _processFlowInstances.Remove(_processFlowInstances.First(p => p.Id == processFlowInstance.Id));
                _processFlowInstances.Add((ProcessFlowInstance)processFlowInstance.Clone());
            }
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
