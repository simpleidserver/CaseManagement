using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Persistence.Parameters;
using CaseManagement.Workflow.Persistence.Responses;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence
{
    public interface IProcessFlowInstanceQueryRepository
    {
        Task<ProcessFlowInstance> FindFlowInstanceById(string id);
        Task<FindResponse<ProcessFlowInstanceExecutionStep>> FindExecutionSteps(FindExecutionStepsParameter parameter);
        Task<FindResponse<ProcessFlowInstance>> Find(FindWorkflowInstanceParameter parameter);
    }
}
