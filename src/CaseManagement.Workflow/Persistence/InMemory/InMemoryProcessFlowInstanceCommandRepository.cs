using CaseManagement.Workflow.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence.InMemory
{
    public class InMemoryProcessFlowInstanceCommandRepository : IProcessFlowInstanceCommandRepository
    {
        private readonly ICollection<ProcessFlowInstance> _processFlowInstances;

        public InMemoryProcessFlowInstanceCommandRepository(ICollection<ProcessFlowInstance> processFlowInstances)
        {
            _processFlowInstances = processFlowInstances;
        }

        public void Add(ProcessFlowInstance processFlowInstance)
        {
            _processFlowInstances.Add(processFlowInstance);
        }

        public Task<int> SaveChanges()
        {
            return Task.FromResult(1);
        }
    }
}
