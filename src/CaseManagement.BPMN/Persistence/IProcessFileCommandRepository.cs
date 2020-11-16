using CaseManagement.BPMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence
{
    public interface IProcessFileCommandRepository
    {
        Task Add(ProcessFileAggregate processFile, CancellationToken token);
        Task<int> SaveChanges(CancellationToken token);
    }
}
