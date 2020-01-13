using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IWorkflowInstanceCommandRepository
    {
        void Add(Domains.CaseInstance workflowInstance);
        void Update(Domains.CaseInstance workflowInstance);
        Task<int> SaveChanges();
    }
}