using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Engine
{
    public interface IProcessFlowElementProcessor
    {
        string ProcessFlowElementType { get; }
        Task Handle(WorkflowHandlerContext context, CancellationToken token);
    }
}
