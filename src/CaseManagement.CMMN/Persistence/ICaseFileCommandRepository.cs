using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseFileCommandRepository
    {
        void Delete(CaseFileAggregate caseFile);
        void Add(CaseFileAggregate caseFile);
        void Update(CaseFileAggregate caseFile);
        Task<int> SaveChanges();
    }
}
