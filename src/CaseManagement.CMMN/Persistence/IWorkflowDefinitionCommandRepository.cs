using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface IWorkflowDefinitionCommandRepository
    {
        void Update(CaseDefinition workflowDefinition);
        Task<int> SaveChanges();
    }
}
