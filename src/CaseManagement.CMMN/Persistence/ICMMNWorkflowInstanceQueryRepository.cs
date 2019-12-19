using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICMMNWorkflowInstanceQueryRepository
    {
        Task<CMMNWorkflowInstance> FindFlowInstanceById(string id);
    }
}
