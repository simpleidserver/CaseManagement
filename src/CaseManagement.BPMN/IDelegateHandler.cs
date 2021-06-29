using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN
{
    public interface IDelegateHandler
    {
        Task<ICollection<MessageToken>> Execute(ICollection<MessageToken> incoming, DelegateConfigurationAggregate delegateConfiguration, CancellationToken cancellationToken);
    }
}