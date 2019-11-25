using CaseManagement.Workflow.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence.InMemory
{
    public class InMemoryProcessFlowInstanceQueryRepository : IProcessFlowInstanceQueryRepository
    {
        private readonly ICollection<ProcessFlowInstance> _processFlowInstances;

        public InMemoryProcessFlowInstanceQueryRepository(ICollection<ProcessFlowInstance> processFlowInstances)
        {
            _processFlowInstances = processFlowInstances;
        }

        public Task<ProcessFlowInstance> FindFlowInstanceById(string id)
        {
            return Task.FromResult(_processFlowInstances.FirstOrDefault(p => p.Id == id));
        }
    }
}
