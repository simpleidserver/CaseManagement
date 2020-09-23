using CaseManagement.Common.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Processors
{
    public interface IProcessor<TExec, TElt, TInst> where TExec : ExecutionContext<TInst> where TInst : BaseAggregate
    {
        Task<ExecutionResult> Execute(TExec executionContext, TElt elt, CancellationToken cancellationToken);
    }
}
