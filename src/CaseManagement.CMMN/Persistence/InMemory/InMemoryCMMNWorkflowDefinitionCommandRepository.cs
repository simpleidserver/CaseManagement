using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.InMemory
{
    public class InMemoryCMMNWorkflowDefinitionCommandRepository : ICMMNWorkflowDefinitionCommandRepository
    {
        public InMemoryCMMNWorkflowDefinitionCommandRepository()
        {

        }

        public Task<int> SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void Update(CMMNWorkflowDefinition workflowDefinition)
        {
            throw new System.NotImplementedException();
        }
    }
}
