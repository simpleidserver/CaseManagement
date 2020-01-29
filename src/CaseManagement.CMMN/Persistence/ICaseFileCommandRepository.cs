using CaseManagement.CMMN.Domains.CaseFile;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseFileCommandRepository
    {
        void Delete(CaseFileDefinitionAggregate caseFile);
        void Add(CaseFileDefinitionAggregate caseFile);
        void Update(CaseFileDefinitionAggregate caseFile);
        Task<int> SaveChanges();
    }
}
