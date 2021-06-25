using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Activities.Handlers
{
    public interface IServiceTaskHandler
    {
        string Implementation { get; }
        Task<ICollection<MessageToken>> Execute(BPMNExecutionContext context, ServiceTask serviceTask, CancellationToken cancellationToken);
    }
}
