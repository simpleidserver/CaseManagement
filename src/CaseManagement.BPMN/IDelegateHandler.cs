using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.ProcessInstance.Processors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN
{
    public interface IDelegateHandler
    {
        Task<ICollection<MessageToken>> Execute(BPMNExecutionContext context, ICollection<MessageToken> incoming, DelegateConfigurationAggregate delegateConfiguration, CancellationToken cancellationToken);
    }
}