using CaseManagement.CMMN.Domains;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseFileCommandRepository
    {
        Task<CaseFileAggregate> Get(string id, CancellationToken token);
        Task Delete(CaseFileAggregate caseFile, CancellationToken token);
        Task Add(CaseFileAggregate caseFile, CancellationToken token);
        Task Update(CaseFileAggregate caseFile, CancellationToken token);
        Task<int> SaveChanges(CancellationToken token);
    }
}
