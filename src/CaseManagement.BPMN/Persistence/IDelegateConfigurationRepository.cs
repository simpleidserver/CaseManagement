using CaseManagement.BPMN.Domains.DelegateConfiguration;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence
{
    public interface IDelegateConfigurationRepository
    {
        Task<DelegateConfigurationAggregate> Get(string delegateId, CancellationToken cancellationToken);
    }
}
