using CaseManagement.Workflow.Domains;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence
{
    public interface IProcessFlowInstanceQueryRepository
    {
        Task<ProcessFlowInstance> FindFlowInstanceById(string id);
    }
}
