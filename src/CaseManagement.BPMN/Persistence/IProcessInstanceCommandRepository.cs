using CaseManagement.BPMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence
{
    public interface IProcessInstanceCommandRepository
    {
        Task Add(ProcessInstanceAggregate processInstance, CancellationToken token);
        Task Update(ProcessInstanceAggregate processInstance, CancellationToken token);
        Task<int> SaveChanges(CancellationToken token);
    }
}
