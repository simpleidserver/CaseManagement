using CaseManagement.BPMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence
{
    public interface IProcessFileQueryRepository
    {
        Task<ProcessFileAggregate> Get(string id, CancellationToken token);
    }
}
