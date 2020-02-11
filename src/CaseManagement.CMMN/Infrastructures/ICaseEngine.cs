using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures
{
    public interface ICaseEngine
    {
        Task Start(CasePlanAggregate workflowDefinition, Domains.CasePlanInstanceAggregate workflowInstance, CancellationToken cancellationToken);
        Task Reactivate(CasePlanAggregate workflowDefinition, Domains.CasePlanInstanceAggregate workflowInstance, CancellationToken cancellationToken);
    }
}
