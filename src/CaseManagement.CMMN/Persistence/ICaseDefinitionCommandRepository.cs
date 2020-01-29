using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseDefinitionCommandRepository
    {
        void Update(CaseDefinition workflowDefinition);
        void Update(CaseDefinitionHistoryAggregate cmmnWorkflowDefinitionStatisticAggregate);
        void Add(CaseDefinitionHistoryAggregate cmmnWorkflowDefinitionStatisticAggregate);
        void Add(CaseDefinition workflowDefinition);
        void Delete(CaseDefinition workflowDefinition);
        Task<int> SaveChanges();
    }
}
