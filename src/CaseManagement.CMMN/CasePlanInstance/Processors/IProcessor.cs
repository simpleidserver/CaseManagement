using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public interface IProcessor<T> where T : BaseCaseEltInstance
    {
        Task Execute(CMMNExecutionContext executionContext, T elt, CancellationToken cancellationToken);
    }
}
