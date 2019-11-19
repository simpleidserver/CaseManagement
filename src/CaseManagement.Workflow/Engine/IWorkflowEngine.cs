using CaseManagement.Workflow.Domains;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Engine
{
    public interface IWorkflowEngine
    {
        Task Start(ProcessFlowInstance processFlowInstance, ProcessFlowInstanceExecutionContext context);
    }
}
