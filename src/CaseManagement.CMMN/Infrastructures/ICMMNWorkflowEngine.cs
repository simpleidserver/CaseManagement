using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public interface ICMMNWorkflowEngine
    {
        Task Start(CMMNWorkflowDefinition workflowDefinition, CMMNWorkflowInstance workflowInstance, CancellationToken cancellationToken);
        Task Reactivate(CMMNWorkflowDefinition workflowDefinition, CMMNWorkflowInstance workflowInstance, CancellationToken cancellationToken);
    }
}
