using CaseManagement.Workflow.Domains;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Engine
{
    public interface IWorkflowEngine
    {
        Task Start(ProcessFlowInstance processFlowInstance);
        Task<bool> Start(ProcessFlowInstance processFlowInstance, ProcessFlowInstanceElement elt);
    }
}
