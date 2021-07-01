using CaseManagement.BPMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public interface IProcessInstanceProcessor
    {
        Task<bool> Execute(ProcessInstanceAggregate processInstance, CancellationToken cancellationToken);
    }
}
