using CaseManagement.Common.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Processors
{
    public interface IProcessor<TInst, TElt> where TInst : BaseAggregate
    {
        Task Execute(ExecutionContext<TInst> executionContext, TElt elt, CancellationToken cancellationToken);
    }
}
