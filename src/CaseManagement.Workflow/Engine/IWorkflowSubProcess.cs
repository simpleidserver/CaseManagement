using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Engine
{
    public interface IWorkflowSubProcess
    {
        Task Task { get; }
        Task Start(WorkflowHandlerContext context, CancellationToken token);
    }
}
