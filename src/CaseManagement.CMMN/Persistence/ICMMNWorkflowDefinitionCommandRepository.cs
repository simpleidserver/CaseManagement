using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICMMNWorkflowDefinitionCommandRepository
    {
        void Update(CMMNWorkflowDefinition workflowDefinition);
        Task<int> SaveChanges();
    }
}
