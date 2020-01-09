using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICMMNWorkflowInstanceQueryRepository
    {
        Task<CMMNWorkflowInstance> FindFlowInstanceById(string id);
        Task<FindResponse<CMMNWorkflowInstance>> Find(FindWorkflowInstanceParameter parameter);
    }
}
