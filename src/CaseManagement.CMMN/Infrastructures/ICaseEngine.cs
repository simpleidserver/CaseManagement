using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public interface ICaseEngine
    {
        Task Start(CaseDefinition workflowDefinition, Domains.CaseInstance workflowInstance, CancellationToken cancellationToken);
        Task Reactivate(CaseDefinition workflowDefinition, Domains.CaseInstance workflowInstance, CancellationToken cancellationToken);
    }
}
