using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public interface ICasePlanInstanceProcessor
    {
        Task Execute(CasePlanInstanceAggregate casePlanInstance, CancellationToken cancellationToken);
    }
}
