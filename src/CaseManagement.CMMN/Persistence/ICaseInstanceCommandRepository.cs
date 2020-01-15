using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseInstanceCommandRepository
    {
        void Add(Domains.CaseInstance workflowInstance);
        void Update(Domains.CaseInstance workflowInstance);
        Task<int> SaveChanges();
    }
}