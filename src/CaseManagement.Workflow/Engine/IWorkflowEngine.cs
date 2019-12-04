using CaseManagement.Workflow.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Engine
{
    public interface IWorkflowEngine
    {
        Task Start(ProcessFlowInstance processFlowInstance, CancellationToken cancellationToken);
    }
}
