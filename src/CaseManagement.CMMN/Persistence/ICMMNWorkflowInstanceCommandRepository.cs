using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICMMNWorkflowInstanceCommandRepository
    {
        void Add(CMMNWorkflowInstance workflowInstance);
        void Update(CMMNWorkflowInstance workflowInstance);
        Task<int> SaveChanges();
    }
}