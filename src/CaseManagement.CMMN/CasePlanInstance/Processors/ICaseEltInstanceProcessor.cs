using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public interface ICaseEltInstanceProcessor
    {
        CasePlanElementInstanceTypes Type { get; }
        Task<ExecutionResult> Execute(CMMNExecutionContext executionContext, CaseEltInstance eltInstance, CancellationToken cancellationToken);
    }
}