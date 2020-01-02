using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCMMNWorkflowInstanceQueryRepository : ICMMNWorkflowInstanceQueryRepository
    {
        private ICollection<CMMNWorkflowInstance> _instances;

        public InMemoryCMMNWorkflowInstanceQueryRepository(ICollection<CMMNWorkflowInstance> instances)
        {
            _instances = instances;
        }

        public Task<CMMNWorkflowInstance> FindFlowInstanceById(string id)
        {
            return Task.FromResult(_instances.FirstOrDefault(i => i.Id == id));
        }
    }
}
