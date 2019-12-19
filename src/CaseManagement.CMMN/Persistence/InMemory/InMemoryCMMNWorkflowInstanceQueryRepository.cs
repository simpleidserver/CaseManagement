using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCMMNWorkflowInstanceQueryRepository : ICMMNWorkflowInstanceQueryRepository
    {
        public Task<CMMNWorkflowInstance> FindFlowInstanceById(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
