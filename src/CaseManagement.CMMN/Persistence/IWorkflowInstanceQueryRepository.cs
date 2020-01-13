using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IWorkflowInstanceQueryRepository
    {
        Task<Domains.CaseInstance> FindFlowInstanceById(string id);
        Task<FindResponse<Domains.CaseInstance>> Find(FindWorkflowInstanceParameter parameter);
    }
}
