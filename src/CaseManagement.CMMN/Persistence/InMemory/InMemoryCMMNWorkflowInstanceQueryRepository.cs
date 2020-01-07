using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            lock(_instances)
            {
                var result = _instances.FirstOrDefault(i => i.Id == id);
                if (result == null)
                {
                    return Task.FromResult((CMMNWorkflowInstance)null);
                }

                return Task.FromResult(result.Clone()  as CMMNWorkflowInstance);
            }
        }
    }
}
