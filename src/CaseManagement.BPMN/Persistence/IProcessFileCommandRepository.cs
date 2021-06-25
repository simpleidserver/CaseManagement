using CaseManagement.BPMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence
{
    public interface IProcessFileCommandRepository
    {
        Task<ProcessFileAggregate> Get(string id, CancellationToken cancellationToken);
        Task Add(ProcessFileAggregate processFile, CancellationToken token);
        Task Update(ProcessFileAggregate processFile, CancellationToken token);
        Task<int> SaveChanges(CancellationToken token);
    }
}
