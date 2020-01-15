using CaseManagement.CMMN.Domains;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence
{
    public interface ICaseDefinitionCommandRepository
    {
        void Update(CaseDefinition workflowDefinition);
        void Update(CaseDefinitionHistoryAggregate cmmnWorkflowDefinitionStatisticAggregate);
        void Add(CaseDefinitionHistoryAggregate cmmnWorkflowDefinitionStatisticAggregate);
        Task<int> SaveChanges();
    }
}
