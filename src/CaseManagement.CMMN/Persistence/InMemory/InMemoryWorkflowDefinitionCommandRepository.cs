using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryWorkflowDefinitionCommandRepository : IWorkflowDefinitionCommandRepository
    {
        public InMemoryWorkflowDefinitionCommandRepository()
        {

        }

        public Task<int> SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void Update(CaseDefinition workflowDefinition)
        {
            throw new System.NotImplementedException();
        }
    }
}
