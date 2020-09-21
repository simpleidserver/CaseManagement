using CaseManagement.Common.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Processors
{
    public interface IProcessorFactory
    {
        Task Execute<TInstance, TElt>(ExecutionContext<TInstance> executionContext, TElt instance, CancellationToken token) where TInstance : BaseAggregate;
    }
}
